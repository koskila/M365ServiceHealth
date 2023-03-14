# M365ServiceHealth

A Blazor WebAssembly site to browse latest M365 Service Announcements.

## Background

This is a quickly hacked together project that I built to try out some new technologies. 
It is a simple web app that displays the current status of the Microsoft 365 service health dashboard, built primarily to try out the new-ish Service Announcements API in Graph API, but also
to get some experience with Static Web Apps.

It is built using the following technologies:
* .NET 7 (and a dash of .NET 6)
* Blazor WebAssembly
* Azure Functions
* Azure Static Web Apps
* GitHub Actions
* ASP.NET Core 7
* Entity Framework Core 7
* Azure SQL Database (even this UI had changed enough to warrant one mroe blog post since I've provisioned a database last time)

It could've been built using a lot less components, but where's the fun in that? 
I suppose you could've built almost all of this using Static Web Apps alone, but I hadn't really used Azure Functions in a while, 
let alone configured GitHub Actions workflows for a variety of different projects.

## Demo

Navigate to https://white-grass-0d81f3a03.2.azurestaticapps.net/

