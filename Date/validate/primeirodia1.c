#include <time.h>
#include <stdio.h>
#include <stdlib.h>






int main(void)
{
 const char *weekdays[] = {"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday"};
 
 const char *monthName[12] = {
                              "January", "February", "March",
                              "April", "May", "June", "July",
                              "August", "September", "October",
                              "November", "December"
                             };
      
 printf("Date (dd/mm/yyyy): ");
 
 int m, y, d;
 
 if(scanf("%d/%d/%d", &d, &m, &y) != 3)
  {
   fprintf(stderr, "Invalid input, brah\n");
   return EXIT_FAILURE;
  }
   
  /* Initialize to a sane default */
  time_t datime = time(NULL);
  struct tm *dadate = localtime(&datime);
  
  /* Apply adjustments based on input */
  dadate->tm_mon = m - 1;
  dadate->tm_mday = d;
  dadate->tm_year = y - 1900;
  
  /* Normalize the adjustments */
  datime = mktime(dadate);
  dadate = localtime(&datime);
    
  /* Rock on! */
  printf("\n\t%d de %s do ano %d foi %s\n", d, monthName[dadate->tm_mon], y, weekdays[dadate->tm_wday]);
 
  return EXIT_SUCCESS;
}