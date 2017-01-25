Standard Library Functions
==========================

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
Takes two conditions. Returns `true` if both conditions evaluate to `true` and
`false` otherwise.

```
(if (and (ge x 13) (le x 19))
    {print "x is in the teens"}
    {print "x is not in the teens"}
)
```

any
---
Takes a function and an array. Returns `true` if the function returns `true`
for any element of the array.

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
Takes a function and an array. Returns `true` if the function returns `true`
for every element of the array.

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
Takes a function and an array. Returns the elements of the array for which the
function returns `true`.

```
(filter is-odd (range 5)) -> {1 3 5}
```

first
-----
Returns the first element of an array.

```
(first {a b c}) -> a
```

foreach
-------
Similar to the `map` function. The only difference is that the array comes
first and the function comes second.

```
(foreach (range 3) square) -> {1 4 9})
```

fun
---
Provides a shortcut for defining named functions compared to using the `def`
function. The `fun` function takes two arguments. The first argument is the
*prototype* of the function, which is an array of symbols containing the name
of the function followed by the names of the parameters. The second argument is
the quoted body of the function.

```
(fun {square x} {mul x x})

[ equivalent function definition using the `def` function ]
(def {square} (lambda {x} {mul x x}))
```

ge
--
Takes two numbers. Returns `true` if the first number is greater than or equal
to the second number and `false` otherwise.

```
(ge 1 2) -> false
(ge 2 2) -> true
(ge 3 2) -> true
```

head
----
Takes an array and returns a slice containing only the first element of the
array.

```
(head {a b c}) -> {a}
```

is-empty
--------
Takes an array and returns `true` if the array is empty and `false` otherwise.

```
(is-empty {}) -> true
(is-empty {a b c}) -> false
```

is-even
-------
Takes a number and returns `true` if the number is even and `false` otherwise.

```
(is-even 1) -> false
(is-even 2) -> true
```

is-odd
------
Takes a number and returns `true` if the number is odd and `false` otherwise.

```
(is-odd 1) -> true
(is-odd 2) -> false
```

last
----
Returns the last element of an array.

```
(last {a b c}) -> c
```

le
--
Takes two numbers. Returns `true` if the first number is less than or equal to
the second number and `false` otherwise.

```
(le 1 2) -> true
(le 2 2) -> true
(le 3 2) -> false
```

map
---
Takes a function and an array. Applies the function to every element of the
array and returns an array containing the results.

```
(map square (range 3)) -> {1 4 9}
```

ne
--
Takes two arguments. Returns `true` if the arguments are *not* equal and
`false` otherwise.

```
(ne 1 0) -> true
```

not
---
Takes a boolean and returns the opposite value.

```
(not true) -> false
(not false) -> true
```

or
--
Takes two conditions. Returns `true` if either condition evaluates to `true`
and `false` otherwise.

```
(if (or (gt x 0) (lt x 0))
    {print "x is nonzero"}
    {print "x is zero"}
)
```

product
-------
Returns the product of an array of numbers.

```
(product {2 3 5}) -> 30
```

range
-----
Takes a number and returns an array of numbers from 1 to that number.

```
(range 3) -> {1 2 3}
```

reduce
------
Takes a function, an array, and an accumulator. Applies the function against
the accumulator and each element of the array to reduce the array to a single
value.

```
(reduce add {1 2 3} 0) -> 6

[ equivalent expression ]
(add (add (add 0 1) 2) 3) -> 6
```

reverse
-------
Reverses an array.

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
Returns the sum of an array of numbers.

```
(sum {2 3 5}) -> 10
```

tail
----
Takes an array and returns a slice containing all but the first element of the
array.

```
(tail {1 2 3}) -> {2 3}
```
