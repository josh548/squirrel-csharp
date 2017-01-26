Language Reference
==================

Table of Contents
-----------------
- [Comments](#comments)
- [Integers](#integers)
- [Symbols](#symbols)
- [Strings](#strings)
- [Booleans](#booleans)
- [Null](#null)
- [Symbolic Expressions](#symbolic-expressions)
- [Quoted Expressions](#quoted-expressions)
- [Arrays](#arrays)
- [Builtin Functions](#builtin-functions)
- [Lambda Functions](#lambda-functions)
- [Modules](#modules)

Comments
--------
Comments begin with a left square bracket and end with a right square bracket.

```
[ this is a line comment ]

[
  and this is a
  block comment
]
```

Integers
--------
Integers are the only numeric type currently supported. Negative integers are
preceded by a minus sign. Positive integers can be preceded by an optional plus
sign.

```
1 -2 +3
```

Symbols
-------
A symbol is an object with a unique name. A symbol can serve as a variable, a
function name, or as data on its own. A symbol name is a case-sensitive string
of letters and, optionally, hyphens, although it cannot begin or end with a
hyphen.

```
foo     [ a symbol named 'foo' ]
FOO     [ a symbol named 'FOO', different from 'foo' ]
foo-bar [ a symbol with hyphens in its name ]
```

Booleans
--------
The symbols `true` and `false` represent boolean values. Functions that operate
on or return boolean values should use these symbols.

Null
----
The symbol `null` represents a meaningless value. Functions that do not
need to return a meaningful value should return `null`.

Strings
-------
Strings are enclosed in double quotes. Line breaks can be inserted with `\n`.
Double quotes inside a string must be escaped with a backslash.

```
"hello world"
"this ends with a line break\n"
"\"Squirrel\" is the name of a programming language"
```

Symbolic Expressions
--------------------
A symbolic expression (or *s-expression*) is a parenthesized list of items, of
which the first item is a function, and any remaining items are treated as
arguments.

```
(add 1 2) [ -> 3 | function: add, arguments: 1 2 ]
```

The items inside an s-expression are evaluated before the s-expression itself.

```
(add 1 (mul 2 3)) [ -> (add 1 6) -> 7 ]
```

Quoted Expressions
------------------
A quoted expression (or *q-expression*) is a braced list of items. A
q-expression evaluates to itself. Q-expressions are frequently used to prevent
the evaluation of certain expressions.

```
{add 1 2} [ -> {add 1 2} ]

{add 1 (mul 2 3}} [ -> {add 1 (mul 2 3}} ]
```

Arrays
------
Q-expressions serve as arrays because they prevent the evaluation of the items
they contain.

```
{one 2 "three"} [ array containing three elements of various types ]
```

Builtin Functions
-----------------
Builtin functions are functions that are implemented as part of the core of the
language (i.e., the interpreter). Functions are invoked using *prefix*
notation. A function call is written as a symbolic expression containing the
name of the function followed by any arguments.

For a list of builtin functions, see [here](builtin-functions.md).

Lambda Functions
----------------
A lambda function is an anonymous function created using the builtin `lambda`
function. The `lambda` function takes two arguments, both q-expressions. The
first argument is an array of symbols, which serve as the names of the function
parameters. The second argument is the body of the function. Because the body
of the function is a q-expression, it does not get evaluated prematurely
before the function is called.

```
[ function that returns the square of a number ]
(lambda {x} {mul x x})

[ function that returns the average of two numbers ]
(lambda {x y} {div (add x y) 2})
```

Lambda functions are invoked like builtin functions: as the first element of an
s-expression followed by any arguments.

```
((lambda {x} {mul x x}) 5) [ -> 25 ]

((lambda {x y} {div (add x y) 2}) 10 20) [ -> 15 ]
```

Modules
-------
Modules are groups of definitions (variables and functions) residing in
separate source files. Modules can be defined with the builtin `module`
function.

```
[ example-module.sq ]
(module
    (def {square} (lambda {x} {mul x x}))
)
```

Module definitions can be included in other source files with the builtin
`include` function, which takes the name of the module as a string, but without
the *.sq* extension. The interpreter searches for the module in a list of
directories specified with the `-i | --include` command line option.

```
[ using-modules.sq ]
(block
    (include "example-module")
    (square 3)
) -> 9
```
