Builtin Functions
=================

add
---
Returns the sum of a sequence of numbers.

```
(add 1 2 3) -> 6
```

block
-----
Takes a sequence of expressions and returns the result of the last expression of the sequence.

```
(block
    (def {x y} {1 2})
    (add x y)
) -> 3
```

def
---
Takes a list of symbols followed by a sequence of values and binds the values to the symbols in the current scope. Bound values can be accessed from inner scopes, but not outer scopes.

```
(block
    (def {x y} 1 2)
    (add x y)
) -> 3

[ results in an error because x and y are undefined in this scope ]
(add x y)
```

A symbol bound in a certain scope will shadow the same symbol bound in an outer scope.

```
(def {x} 2)

(block
    (def {x} 3)
    (mul x x)
) -> 9

[ the inner binding has no effect on the outer binding ]
(mul x x) -> 4
```

div
---
Returns the quotient of two numbers.

```
(div 6 3) -> 2
```

eq
--
Takes two arguments and returns `true` if the arguments are equal and `false` otherwise. The arguments are considered equal if they evaluate to the same value. The arguments can be of any type.

```
(eq 6 (add 1 2 3)) -> true
```

eval
----
Takes a list as its only argument. If the list contains a single item, `eval` returns that item. If the list contains more than one item, `eval` returns the value of the list as if it were a symbolic expression.

```
(eval {add}) -> add
(eval {add 1 2}) -> (add 1 2) -> 3
```

gt
--
Takes two numbers and returns `true` if the first number is greater than the second, otherwise returns `false`.

```
(gt 1 2) -> false
```

head
----
Returns the first item of a list.

```
(head {a b c}) -> a
```

join
----
Concatenates a sequence of lists.

```
(join {a b} {c} {d e f}) -> {a b c d e f}
```

lambda
------
Takes two arguments, both quoted expressions. The first argument is a list of symbols, which are the function parameters. A function must have at least one parameter. The second argument is the body of the function.

```
[ function that returns the square of a number ]
(lambda {x} {mul x x})
```

Use `lambda` in conjunction with `def` to create named functions.

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
Returns the length of a list.

```
(len {a b c}) -> 3
```

lt
--
Takes two numbers and returns `true` if the first number is less than the second, otherwise returns `false`.

```
(lt 1 2) -> true
```

mod
---
Returns the remainder after division of two numbers.

```
(mod 5 3) -> 2
```

mul
---
Returns the product of a sequence of numbers.

```
(mul 2 3 5) -> 30
```

nth
---
Returns the nth item of a list.

```
(nth {a b c} 2) -> b
```

outer
-----
Almost exactly like `def`. The only difference is that `outer` binds values to symbols in the outer scope rather than the current scope.

```
(block
    (outer {a b c} 1 2 3)
    [ using a, b, or c in this scope would result in an error]
)
(add a b c) -> 6
```

print
-----
Takes a string argument and prints it to the console. Does *not* print a newline after the string. To print a newline, end the string with a newline character.

```
(print "example of the \"print\" function\n") -> example of the "print" function
```

quote
-----
Returns a list of the arguments.

```
(quote a b c) -> {a b c}
```

sub
---
Returns the difference of two numbers.

```
(sub 3 2) -> 1
```

tail
----
Returns all but the first item of a list.

```
(tail {1 2 3}) -> {2 3}
```

when
----
Takes a variable number of arguments called  *clauses*. Each clause is a list containing two items, a *condition* and a *result*. The `when` function returns the result of the first clause for which the condition is true.

```
(def {x} (add 1 1))
(when
    {(eq x 1) wrong}
    {(eq x 2) right}
    {(eq x 3) wrong}
) -> right
```

