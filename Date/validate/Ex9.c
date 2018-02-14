#include <stdio.h>



























int main()
{
 enum months{ January=1, February, March, April, May, June, July, August, September, October, November, December}month;
 
 const char *monthName[12] = {
                              "January", "February", "March",
                              "April", "May", "June", "July",
                              "August", "September", "October",
                              "November", "December"
                             };
   
 for(month = January; month <= December; month++)
 printf("%2d%11s\n", month, monthName[month-1]);

 return 0;
}