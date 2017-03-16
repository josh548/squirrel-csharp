# Squirrel
Squirrel is an expression-oriented programming language inspired by Lisp.

Check out the [language reference](docs/language-reference.md) to learn how it
works.

## Sample

```
[ program that defines a function,
  invokes it, and displays the result ]
(block
  (def {factorial}
    (lambda {x}
      {if (eq x 0)
        {id 1}
        {mul x (factorial (sub x 1))}
      }
    )
  )
  (display (factorial 5)) [ prints 120 ]
)
```

## Prerequisites
Install [.NET Core](https://www.microsoft.com/net/core) 1.1 or later in order
to build and run the project.

## Running

```
cd src/app/
dotnet restore

# run an interactive Squirrel console
dotnet run

# run a Squirrel source file
dotnet run <path>

# run one of the included modules: Conway's Game of Life
dotnet run ../../modules/game-of-life.sq
```

## Testing

```
cd test/test-library/
dotnet restore
dotnet test
```

## Resources
- [Language Reference](docs/language-reference.md)
- [List of Builtin Functions](docs/builtin-functions.md)
- [List of Standard Library Functions](docs/standard-library-functions.md)
