# Squirrel
Squirrel is an interpreted programming language inspired by Lisp.

## Prerequisites
Install [.NET Core](https://www.microsoft.com/net/core) 1.1 or later in order
to build and run the project.

## Building

```
git clone https://github.com/escamilla/squirrel
cd squirrel/
dotnet restore
```

## Running

```
cd src/app/

# run an interactive Squirrel console
dotnet run

# run a Squirrel source file
dotnet run <path>
```

## Testing

```
cd test/test-library/
dotnet test
```

## Resources
- [Language Reference](docs/language-reference.md)
- [List of Builtin Operators](docs/builtin-operators.md)
- [List of Standard Library Functions](docs/standard-library-functions.md)
