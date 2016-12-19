# Squirrel
Squirrel is an interpreted programming language with a minimal syntax inspired by Lisp.

## Table of Contents
- [Comments] (#comments)
- [Integers] (#integers)
- [Symbols] (#symbols)
- [Booleans] (#booleans)
- [Symbolic expressions] (#symbolic-expressions)
- [Quoted expressions] (#quoted-expressions)
- [Lambda functions] (#lambda-functions)
- [Defining values] (#defining-values)

## Comments
Comments are enclosed in square brackets and can span multiple lines.

```
[ this is a line comment ]

[
  and this is a
  block comment
]
```

## Integers
Integers are the only numeric type currently supported. Negative integers are preceded by a minus sign. Positive integers can be preceded by an optional plus sign.
```
1 +2 -3
```

## Symbols
A symbol is a case-sensitive word consisting only of alphabetic characters.

```
add sub mul div
```

## Booleans
The symbols `true` and `false` act as boolean values. Boolean functions should return one of these symbols.

```
(eq (add 1 2) 3) [ -> true ])
(eq 0 (len {a b c})) [ -> false ])
```

## Symbolic expressions
A symbolic expression (*s-expression* for short) is a sequence of expressions enclosed in parentheses where the first expression is a symbol. The symbol is treated as an operator and the remaining expressions are treated as operands.

```
(add 1 2) [ operator: add | operands: 1 2 ]
```

Nested expressions are evaluated before the whole expression.
```
(add 1 (mul 2 3)) [ operator: add | operands: 1 6 ]
```

## Quoted expressions
A quoted expression (*q-expression* for short) is a sequence of expressions enclosed in curly braces. Q-expressions evaluate to themselves, and nested expressions are not evaluated at all.

```
{add 1 2} [ -> {add 1 2} ]

{add 1 (mul 2 3}} [ -> {add 1 (mul 2 3}} ]
```

Because expressions nested inside q-expressions are not evaluated, q-expressions can be used as lists for storing data.

## Lambda functions
A lambda function can be created using the builtin `lambda` keyword. The `lambda` keyword takes two arguments, which are both q-expressions. The first argument is the parameter list and the second argument is the body of the function.

```
[ function that returns the square of a number ]
(lambda {x} {mul x x})

[ function that returns the average of two numbers ]
(lambda {x y} {div (add x y) 2})
```

A lambda function can be evaluated by placing it inside an s-expression followed by the arguments.

```
((lambda {x} {mul x x}) 5) [ -> 25 ]

((lambda {x y} {div (add x y) 2}) 10 20) [ -> 15 ]
```

## Defining values
Constant values can be defined using the builtin `def` keyword. The first argument to the `def` keyword is a list (q-expression) of symbols which are the names to define. The remaining arguments are the values to associate with each name.

```
[ defining a single value ]
(def {x} 5) [ x = 5 ]

[ defining multiple values ]
(def {y z} (mul x x) {a b c}) [ y = 25 | z = {a b c} ]

[ defining a named function ]
(def {square} (lambda {x} {mul x x}))
(square 5) [ -> 25 ]
```
