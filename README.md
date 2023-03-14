# M365ServiceHealth

A Blazor WebAssembly site to browse latest M365 Service Announcements. These are tenant-scoped announcements that are sent to users via the Office 365 Message Center.
By default, normal users don't really get visibility to this data - and even admins are limited to just seeing their tenant's announcements with little additional context.

This little project reads Service Announcements from multiple tenants, and presents them in a single view. It also provides a little more context around the announcements, such as
where has it been encountered (data center geographical locations), when was it first seen, and how many times it's been updated.

## Background

I mostly build this to try out some new technologies. 
It is a simple web app that displays the current status of the Microsoft 365 service health dashboard, 
built primarily to try out the new-ish Service Announcements API in Graph API, but also
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

It could've been built using a lot less components, but where's the fun in that? 😎 
Almost all of this could've been constructed using Static Web Apps alone, but I hadn't really used Azure Functions in a while, 
let alone configured GitHub Actions workflows for a variety of different projects.

## Demo

Navigate to https://white-grass-0d81f3a03.2.azurestaticapps.net/. If GitHub Actions has run (it just broke due to runner updates, but it's probably up soon again), it should show the latest version of the app.

