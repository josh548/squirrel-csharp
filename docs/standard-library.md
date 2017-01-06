Standard Library
----------------

abs
---
Returns the absolute value of a number.

```
(abs -1) -> 1
(abs 0)  -> 0
(abs +1) -> 1
```

and
---
Takes two conditions. Returns `true` if both conditions evaluate to `true`, otherwise returns `false`.

```
(if (and (ge x 13) (le x 19))
    {print "x is in the teens"}
    {print "x is not in the teens"}
)
```

any
---
Takes a function and a list. Returns `true` if the function returns `true` for any item of the list.

```
(any is-odd {2 3 4}) -> true
```

cube
----
Returns the cube of a number.

```
(cube 3) -> 27
```

every
-----
Takes a function and a list. Returns `true` if the function returns `true` for every item of the list.

```
(every is-even {2 4 6}) -> true
```

factorial
---------
Returns the factorial of a number.

```
(factorial 5) -> 120
```

filter
------
Takes a function and a list. Returns the items of the list for which the function returns `true`.

```
(filter is-odd (range 5)) -> {1 3 5}
```

first
-----
Returns the first item of a list.

```
(first {a b c}) -> a
```

foreach
-------
Almost exactly like `map`. The only difference is that the list comes first and the function comes second.

```
(foreach (range 3) square) -> {1 4 9})
```

fun
---
Provides a shortcut for defining named functions compared to using the `def` operator. The `fun` operator takes two arguments. The first argument is the *prototype* of the function, which is a list of symbols containing the name of the function followed by its parameters. The second argument is the body of the function as a quoted expression.

```
(fun {square x} {mul x x})

[ equivalent function definition using `def` ]
(def {square} (lambda {x} {mul x x}))
```

ge
--
Takes two numbers. Returns `true` if the first number if greater than or equal to the second number, otherwise returns `false`.

```
(ge 1 2) -> false
(ge 2 2) -> true
(ge 3 2) -> true
```

if
--
Takes a *condition* and two *outcomes*. The condition must be an expression that evaluates to either `true` or `false`. The outcomes must be quoted expressions. If the condition evaluates to `true`, the first outcome will be evaluated. If the condition evaluates to `false`, the second outcome will be evaluated.

```
(if (lt x 0)
    {print "x is negative"}
    {print "x is either positive or zero"}
)
```

is-empty
--------
Takes a list and returns `true` if the list is empty, otherwise returns `false`.

```
(is-empty {}) -> true
(is-empty {a b c}) -> false
```

is-even
-------
Takes a number and returns `true` if the number is even, otherwise returns `false`.

```
(is-even 1) -> false
(is-even 2) -> true
```

is-odd
------
Takes a number and returns `true` if the number is odd, otherwise returns `false`.

```
(is-odd 1) -> true
(is-odd 2) -> false
```

last
----
Returns the last item of a list.

```
(last {a b c}) -> c
```

le
--
Takes two numbers. Returns `true` if the first number if less than or equal to the second number, otherwise returns `false`.

```
(le 1 2) -> true
(le 2 2) -> true
(le 3 2) -> false
```

map
---
Takes a function and a list. Applies the function to every item of the list and returns the results.

```
(map square (range 3)) -> {1 4 9}
```

ne
--
Takes two arguments. Returns `true` if the arguments are equal and `false` otherwise. Returns the opposite value as the `eq` operator.

```
(ne 1 0) -> true
```

not
---
Takes a boolean condition and returns the opposite value.

```
(not true) -> false
(not false) -> true
```

or
--
Takes two conditions. Returns `true` if either condition evaluates to `true`, otherwise returns `false`.

```
(if (or (gt x 0) (lt x 0))
    {print "x is nonzero"}
    {print "x is zero"}
)
```

product
-------
Returns the product of a list of numbers.

```
(product {2 3 5}) -> 30
```

range
-----
Takes a number and returns a list of numbers from one to that number.

```
(range 3) -> {1 2 3}
```

reduce
------
Takes a function, a list, and an accumulator. Applies the function against the accumulator and each item of the list to reduce the list to a single value.

```
(reduce add {1 2 3} 0) -> 6

[ equivalent expression ]
(add (add (add 0 1) 2) 3) -> 6
```

reverse
-------
Reverses a list.

```
(reverse {a b c}) -> {c b a})
```

square
------
Returns the square of a number.

```
(square 3) -> 9
```

sum
---
Returns the sum of a list of numbers.

```
(sum {2 3 5}) -> 10
```
