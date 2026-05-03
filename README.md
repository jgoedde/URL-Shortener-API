# URL Shortener API

A learning project exploring backend engineering and system design concepts using .NET - clean architecture, CQRS, distributed caching, and load testing.

## Architecture

<img width="1215" height="950" alt="Untitled-2026-05-02-1209" src="https://github.com/user-attachments/assets/114dfa2e-e169-4ecc-9fe3-6557a0abb6a2" />

## Run the system

```shell
docker-compose up --build
```

### Services

- API: http://localhost:8080
  - Swagger: http://localhost:8080/swagger
- Seq (logs): http://localhost:5341
  - The initial admin password is `seqadmin`
- PostgreSQL: localhost:5432
- Redis: localhost:6379

## Load Testing

Load tests are written in [k6](https://k6.io) and live in `/load-tests`.

### Running

```bash
k6 run -e BASE_URL="http://localhost:5000" load-tests/src/<script-name>.js
```

### Results

<details open>
<summary>GET /{shortCode} — Redirect, 2026-05-02 (with Redis caching)</summary>

**Parameters:**

- ramp up to 50 VUs → spike to 200 VUs
- Release Mode
- local PostgreSQL + Redis
- randomized short codes
- commit: [`e0f1ae0b1d38187e7ae4a0ac3a2a0a8df1340a0f`](https://github.com/jgoedde/URL-Shortener-API/commit/e0f1ae0b1d38187e7ae4a0ac3a2a0a8df1340a0f)

| Metric     | Value       |
|------------|-------------|
| Avg        | 1.71ms      |
| p(95)      | 4.61ms      |
| p(99)      | 8.61ms      |
| Max        | 278.35ms    |
| Throughput | 79.26 req/s |
| Error rate | 0.00%       |

**Thresholds**
- ✅ `p(95) < 50ms`
- ✅ `p(99) < 100ms`
- ✅ `error rate < 1%`

</details>

<details>
<summary>GET /{shortCode} — Redirect, 2026-05-02 (without caching)</summary>

**Parameters:**

- ramp up to 50 VUs → spike to 200VUs
- Release Mode
- local PostgreSQL
- randomized short codes
- commit: [`affd494efd656fbdf32fcbf45d1c1e69498b58d9`](https://github.com/jgoedde/URL-Shortener-API/commit/affd494efd656fbdf32fcbf45d1c1e69498b58d9)

| Metric     | Value       |
|------------|-------------|
| Avg        | 8.64ms      |
| p(95)      | 13.48ms     |
| p(99)      | 77.09ms     |
| Max        | 768.14ms    |
| Throughput | 79.01 req/s |
| Error rate | 0.00%       |

**Thresholds**
- ✅ `p(95) < 50ms`
- ✅ `p(99) < 100ms`
- ✅ `error rate < 1%`
- **No caching yet!**

</details>


<details>
<summary>POST /api/urls - Shorten URL, 2026-05-01</summary>

**Parameters:**

- 20 VUs
- 1m45s
- local PostgreSQL
- Release Mode
- commit [`31ca1b7161134cea256577867451a931b1898cbb`](https://github.com/jgoedde/URL-Shortener-API/commit/31ca1b7161134cea256577867451a931b1898cbb)

| Metric     | Value       |
|------------|-------------|
| Avg        | 57.37ms     |
| p(95)      | 80.25ms     |
| p(99)      | 100.27ms    |
| Max        | 670.76ms    |
| Throughput | 14.82 req/s |
| Error rate | 0.00%       |

**Thresholds**
- ✅ `p(95) < 300ms`
- ✅ `p(99) < 500ms`
- ✅ `error rate < 1%`

</details>

## Prerequisites

This solution in built on the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0), you need to install that before it will work for you.  If you want to build the Dockerfile you will need to install [Docker](https://www.docker.com/products/docker-desktop) as well.

## Architecture

This solution is loosely based on Clean Architecture patterns, it's by no means perfect.  I prefer to call it "Lean Mean Clean Architecture".  Inspiration has been taken from [Jason Taylor's Clean Architecture Template](https://github.com/jasontaylordev/CleanArchitecture), but I have made some structural decisions to take some things further and scaled back others.

There's a little CQRS type stuff going on here but it's more in style than real separated functions for reading and writing as under the covers they are the same data source.

Breaking the Clean Architecture pattern is the fact that the Infrastructure project is referenced by the Presentation project.  This is for **Dependency Injection** purposes, so to protect this a little further, all classes in the Infrastructure project are `internal`.  This stops them being accidentally used in the Presentation project.

### Project Structure

It's streamlined into 3 functional projects.  All serve their own purpose and segregate aspects of the application to allow easier replacement and updating.

1. **Presentation** - Setting up the interactions between the Application layer and the consumer.  In the project that's via a Minimal API but it could be many other things.  The Minimal API uses endpoints to funnel the actions to the layer that owns the domain.
1. **Application** - This project owns the domain and business logic.  There's validation of the Commands and Queries and handling of domain entities in their own separated structures.  Each domain type has it's own interface to a datasource downstream, this project doesn't care what fulfills this contract, as long as someone does.
1. **Infrastructure** - Here's where the database comes into play.  Infra owns the data objects and works with the repository interfaces to fetch, create, update and remove object from the source.  There's some entity mapping here to allow specific models with attributes to remain in this layer and not bleed through to the **Application** layer.

## Features

There are plenty of handy implementations of features throughout this solution, in no particular order here are some that might interest you.

- Logging using [Serilog](https://github.com/serilog/serilog)
- Mediator Pattern using [Mediatr](https://github.com/jbogard/MediatR)
- Validation using [FluentValidation](https://github.com/FluentValidation/FluentValidation)
- ~~Testing using [Shouldly](https://github.com/shouldly/shouldly) and [NSubstitute](https://github.com/nsubstitute/NSubstitute)~~
- OpenApi using [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
- ~~Object Mapping using [AutoMapper](https://github.com/AutoMapper/AutoMapper)~~

## Resources

This sample would not have been possible without gaining inspiration from the following resources.  If you are on your own learning adventure please read the following blogs and documentation.

- [David Fowler - Minimal APIs at a glance](https://gist.github.com/davidfowl/ff1addd02d239d2d26f4648a06158727)
- [Damian Edwards - Minimal API Playground](https://github.com/DamianEdwards/MinimalApiPlayground)
- [Scott Hanselman - Minimal APIs in .NET 6 but where are the Unit Tests?](https://www.hanselman.com/blog/minimal-apis-in-net-6-but-where-are-the-unit-tests)
- [Andrew Lock - Reducing log verbosity with Serilog RequestLogging](https://andrewlock.net/using-serilog-aspnetcore-in-asp-net-core-3-reducing-log-verbosity/)
- [Ben Foster - Minimal API validation with ASP.NET 7.0 Endpoint Filters](https://benfoster.io/blog/minimal-api-validation-endpoint-filters/)

## Developing

Set connection string on local dev machine:

```bash
dotnet user-secrets set "ConnectionStrings:Default" "Host=localhost;Database=app;Username=postgres;Password=dev"
dotnet user-secrets set "ConnectionStrings:Redis" "localhost:6379"
```
