Builtin Operators
=================

add
---
Returns the sum of a sequence of numbers.

```
(add 1 2 3) -> 6
```

block
-----
Takes a sequence of expressions and returns the value of the last expression of
the sequence.

```
(block
    (def {x y} {1 2})
    (add x y)
) -> 3
```

def
---
Takes an array of symbols followed by a sequence of values and binds the values
to the symbols in the current scope. Bound values can be accessed from inner
scopes, but not from outer scopes. Returns `null`.

```
(block
    (def {x y} 1 2)
    (add x y)
) -> 3

[ results in an error because x and y are undefined in this scope ]
(add x y)
```

A symbol bound in a certain scope will shadow the same symbol bound in an outer
scope.

```
(def {x} 2)

(block
    (def {x} 3)
    (mul x x)
) -> 9

[ the inner binding has no effect on the outer binding ]
(mul x x) -> 4
```

display
-------
Prints the string representation of an object to the console, which is a string
that, when interpreted, evaluates to an equivalent object. Returns `null`. For
printing strings, see [`print`](#print).

div
---
Returns the quotient of two numbers.

```
(div 6 3) -> 2
```

eq
--
Takes two arguments and returns `true` if the arguments are equal and `false`
otherwise. The arguments are considered equal if they evaluate to the same
value. The arguments can be of any data type.

```
(eq 6 (add 1 2 3)) -> true
```

eval
----
Takes an array. If the array contains a single element, `eval` returns that
element. If the array contains more than one element, `eval` returns the value
of the array evaluated as if it were an s-expression.

```
(eval {add}) -> add
(eval {add 1 2}) -> (add 1 2) -> 3
```

gt
--
Takes two numbers and returns `true` if the first number is greater than the
second and `false` otherwise.

```
(gt 1 2) -> false
```

include
-------
Takes one argument, the path to a module source file. Loads definitions from
the module. Returns `null`. See [`module`](#module).

join
----
Concatenates a sequence of arrays into a single array.

```
(join {a b} {c} {d e f}) -> {a b c d e f}
```

lambda
------
Takes two arguments, both q-expressions. The first argument is an array of
symbols, which serve as the names of the function parameters. The second
argument is the body of the function.

```
[ function that returns the square of a number ]
(lambda {x} {mul x x})
```

Used in conjunction with the `def` operator to define named functions.

```
(def {factorial} (lambda {x} {
    if (eq x 0)
        {1}
        {mul x (factorial (sub x 1))}
}))

(factorial 5) -> 120
```

len
---
Returns the length of an array.

```
(len {a b c}) -> 3
```

lt
--
Takes two numbers and returns `true` if the first number is less than the
second and `false` otherwise.

```
(lt 1 2) -> true
```

mod
---
Returns the remainder after division of two numbers.

```
(mod 5 3) -> 2
```

module
------
Creates a module whose definitions can be included in other source files with
the `include` operator. Takes a sequence of expressions. After the expressions
are evaluated, definitions are copied from the current environment to the
parent environment. Returns `null`.

```
[ example-module.sq ]
(module
    (def {square} (lambda {x} {mul x x}))
)

[ using-modules.sq ]
(block
    (include "example-module.sq")
    (square 3)
) -> 9
```

mul
---
Returns the product of a sequence of numbers.

```
(mul 2 3 5) -> 30
```

nth
---
Returns the nth element of an array.

```
(nth {a b c} 2) -> b
```

outer
-----
Similar to the `def` operator. The only difference is that `outer` binds values
to symbols in the outer scope rather than the current scope.

```
(block
    (outer {x y} 1 2)
    [ using x or y in this scope would result in an error]
)
(add x y) -> 3
```

print
-----
Prints a string to the console. Returns `null`.

```
(print "hello world!\n")
```

quote
-----
Returns an array containing its arguments.

```
(quote a b c) -> {a b c}
```

set
---
Replaces an element in an array. Takes three arguments. The first argument is a
quoted symbol that is bound to an array. The second argument is the index of
the element to replace in the array. The third argument is the value to replace
the element with.

```
(block
    (def {values} {1 two 3})
    (set {values} 1 2)
    values
) -> {1 2 3}
```

slice
-----
Takes an array and two zero-based indices, a begin index and an end index.
Returns a slice of the array from the begin index up to but not including the
end index.

```
(slice {a b c} 0 1) -> {a}
(slice {a b c} 1 3) -> {b c}
```

sub
---
Returns the difference of two numbers.

```
(sub 3 2) -> 1
```

when
----
Takes a variable number of arguments called *clauses*. Each clause is an array
containing two elements, a *condition* and a *result*. The `when` operator
returns the result of the first clause for which the condition evaluates to
`true`.

```
(def {x} (add 1 1))
(when
    {(eq x 1) wrong}
    {(eq x 2) right}
    {(eq x 3) wrong}
) -> right
```
