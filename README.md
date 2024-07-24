# GoodMatch

This Project was first completed using C# console application.
To add an Interface(UI) I then changed it to Web Applicaction.

- ### The problem :
  
This code calculate the match percentage between two peoples first names.
The sentence to be processed will read as {name1} matches {name2}, for example **'Jack
matches Jill'**

- Starting from the left, it will read the first character in the sentence and count how many times
that character occurs.
-  Move on to the next character that has not been counted yet
-  It will repeat until all characters in the sentence have been counted
-  There will be a number showing how many times each character occurred in the
sentence

#### Eg.
- Jack matches Jill
- ~J~ack matches ~J~ill
  - 2
- ~Ja~ck m~a~tches ~J~ill
  - 22
- ~Jac~k m~a~t~c~hes ~J~ill
  - 222
- ~Jack~ m~a~t~c~hes ~J~ill
  - 2221
- ~Jack~ ~ma~t~c~hes ~J~ill
  - 22211
- ~Jack~ ~ma~t~c~hes ~J~ill
  - 222111
- ~Jack~ ~matc~hes ~J~ill
  - 2221111
- ~Jack~ ~match~es ~J~ill
  - 22211111
- ~Jack~ ~matches~ ~J~ill
  - 222111111
- ~Jack~ ~matches~ ~Ji~ll
  - 2221111111
- ~Jack~ ~matches~ ~Jill~
  - 22211111112


 The  number now "22211111112"needs to be reduced to a 2 digit number.
- Add the left most and right most number that has not been added yet and put its sum at
the end of the result.
- If there is only one number left add that number to the end of the result
- Repeat this process until there are only 2 digits left in the final string

#### Eg.
- ~2~221111111~2~
	- 4
 ~22~2111111~12~
	- 43
 ~222~11111~112~
	- 433
 ~2221~111~1112~
	- 4332
- ~22211~1~11112~
	-43322
- ~22211111112~
	- 433221
- ~4~3322~1~
	- 5
- ~43~32~21~
	- 55
- ~433221~
	- 555
- ~5~5~5~
	- 10
- ~555~
	- 105
- ~1~0~5~
	- 6
- ~105~
	- 60

The result is 60

The output should read **Jack matches Jill 60**

If the percentage is more than 80%, so for example if the output was 82% the output should read
**Jack matches Jill 82%, good match**
