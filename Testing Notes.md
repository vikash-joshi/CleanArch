# Testing Using XUnit

#### [**Fact**]

##### What is `[Fact]`? This tells xUnit "This method is a test." Without it  dotnet test Wont run.

##### Why Async Task as our Handler Uses `Await`.Therefore the test must also be async.

> var uow = Substitute.For<IUnitOfWork>();

Question => Where is SQL Server ?

Answer => Nowhere. This line creates a fake UnitOfWork. Think of it like Real SQL Server ❌ Fake SQL Server ✅

What does Substitute do ?

Normally

> `Handler >> IUnitOfWork >> SQL Server` INSTEAD </span></code></pre></div></div></div></div></div></div></div></div></div></div></div> Handler >> Fake IUnitOfWork >>Memory
