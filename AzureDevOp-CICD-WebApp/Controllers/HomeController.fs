namespace AzureDevOp_CICD_WebApp.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open System.Diagnostics

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

open AzureDevOp_CICD_WebApp.Models
open System.Net
open Microsoft.AspNetCore.Authorization

type HomeController (logger : ILogger<HomeController>) =
    inherit Controller()

     member this.RestrictedPage () =
        let clientIp = IPAddress.Parse("192.168.29.246") //this.HttpContext.Connection.RemoteIpAddress

        // Replace with your actual IP address (e.g., "203.0.113.25")
        let restrictedIp = IPAddress.Parse("192.168.29.246")

        if clientIp.Equals(restrictedIp) then
            this.Redirect("/404.html") :> IActionResult
        else
            this.RedirectToAction("Index") :> IActionResult

            member this.AccessDenied() =
        // Custom condition, e.g., check IP, role, header, etc.
        if not this.User.Identity.IsAuthenticated then
            this.StatusCode(403) :> IActionResult
        else
            this.RedirectToAction("Privacy")

    member this.Index () =
        this.RedirectToAction("AccessDenied")

    [<Authorize(Roles = "Admin")>]
    member this.Privacy () 
        =
        this.View()

    [<HttpGet>]
        

    [<ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)>]
    member this.Error () =
        let reqId = 
            if isNull Activity.Current then
                this.HttpContext.TraceIdentifier
            else
                Activity.Current.Id

        this.View({ RequestId = reqId })
