# ProductManagement — Handoff Prompt (paste this as your first message in a new chat)

I'm following a 16-week .NET career guide (6.6 LPA → 12–18 LPA), building a Clean Architecture project called **ProductManagement**. Here's my progress so far — please continue from here without re-explaining earlier days.

**Current status:** Phase 1, Week 1 — completed through Day 7. Starting Week 2 (Repository Pattern + CRUD) next.

**What's built so far (Days 1–7):**
- Day 1: 4-project solution (Domain, Application, Infrastructure, API) with correct dependency direction. Basic `Product` entity.
- Day 2: `Product` made self-validating (private setters, constructor validation). Added `Money` value object, domain exceptions (`InsufficientStockException`), `ProductCreatedEvent`.
- Day 3: Application-layer contracts — `IProductRepository`, `IUnitOfWork` interfaces, DTOs (`ProductDto`, `CreateProductRequest`), `PagedResult<T>`.
- Day 4: MediatR wired in. First query: `GetProductByIdQuery` + handler. Learned CQRS (commands vs queries).
- Day 5: `CreateProductCommand` + handler. Introduced `Result<T>` pattern for expected failures instead of exceptions.
- Day 6: API layer — `ProductsController` (POST create, GET by id), MediatR registered in `Program.cs`, global exception handler, Swagger UI working. Note: `IUnitOfWork` has no real Infrastructure implementation yet, so it currently needs a stub/fake to run end-to-end.
- Day 7: Week 1 review — README.md written, ASCII architecture diagram added, traced one full request flow (Controller → MediatR → Handler → Domain → back out).

**Also have:** unit tests (xUnit + NSubstitute) for `GetProductByIdQueryHandler` and `CreateProductCommandHandler`.

**My learning style:** I'm a beginner. Please explain things in very simple, plain language with the "why" behind each concept, not just the "how." I like theory → practical code → key insight → deliverable structure for each day.

**What I need help with right now:**
- [ ] Day 8 (or wherever the guide is next) — theory + practical + code
- [ ] Something specific I'm stuck on: ___

---
*(If you want Claude to actually edit the original guide file, also re-upload `16week_dotnet_guide.html`. For just continuing day-by-day teaching, this prompt alone is enough.)*