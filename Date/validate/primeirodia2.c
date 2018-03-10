#include <time.h>
#include <stdio.h>
#include <stdlib.h>





//script que mostra o dia da semana de uma data.
int main(void)
{
 const char *weekdays[7] = {"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"};
 
 const char *monthName[12] = {
                              "January", "February", "March",
                              "April", "May", "June", "July",
                              "August", "September", "October",
                              "November", "December"
                             };
 
 printf("Date (dd/mm/yyyy): ");
 fflush(stdout);
 
 int m, y, d;
 
 if(scanf("%d/%d/%d", &d, &m, &y) != 3)
  {
   fprintf(stderr, "Invalid input, brah\n");
   return EXIT_FAILURE;
  }
 
 /* Initialize to a sane default */
 time_t datime = time(NULL);
 struct tm *dt = localtime(&datime);
 dt->tm_mday = d;
 dt->tm_mon = m-1; 
 dt->tm_year = y-1900;
 dt->tm_isdst = 0;
 datime = mktime(dt); 
 dt = localtime(&datime);
 
 printf("\n\t%d de %s de %d foi %s.\n",d,monthName[dt->tm_mon],y,weekdays[dt->tm_wday]);
 return 0;
}