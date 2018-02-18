#include <iostream>

int main()
{
  time_t now = time(0);
  tm *ltm = localtime(&now);
  
  int day_current(ltm->tm_mday);
  int month_current(ltm->tm_mon+1);
  int year_current(ltm->tm_year+1900);
  
  printf("\n\tThe current year is %4d\n",year_current);
  
  int year;
  
  printf("\n\tEnter the desired year to\n\tcalculate the day of Easter: ");
  scanf("%d", &year);
  
  int a=(year%19);
  int b=int(year/100);
  int c=(year%100);
  int d=int(b/4);
  int e=(b%4);
  int f=int((b+8)/25);
  int g=int((b-f+1)/3);
  int h=((19*a+b-d-g+15)%30);
  int i=int(c/4);
  int k=int(c%4);
  int L=((32+2*e+2*i-h-k)%7);
  int m=int((a+11*h+22*L)/451);
  int month=int((h+L-7*m+114)/31);
  
  enum{ January=1, February, March, April, May, June, July, August, September, October, November, December };
  
  const char *months[12] = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
    
   switch(month)
    { //agora com switch em vez de ifs
     case January: months[month-1]; break;
     case February: months[month-1]; break;
     case March: months[month-1]; break;
     case April: months[month-1]; break;
     case May: months[month-1]; break;
     case June: months[month-1]; break;
     case July: months[month-1]; break;
     case August: months[month-1]; break;
     case September: months[month-1]; break;
     case October: months[month-1]; break;
     case November: months[month-1]; break;
     case December: months[month-1]; break;
    }
 
 int day_easter=(((h+L-7*m+114)%31)+1);
 
 if(year == year_current && month_current == 4 && day_current == day_easter)
 printf("\n\tToday %dth %s %04d is Easter Day",day_easter, months[month-1], year); 
 else
 if(year >= year_current && month_current <= 4 && (day_current < day_easter || day_current > day_easter))
 printf("\n\tEaster will fall. !!\n\t%s %dth, %04d\n\n", months[month-1], day_easter, year);
 else
 printf("\n\tEaster has fallen. !!\n\t%dth %s %04d\n\n", day_easter, months[month-1], year);
 
 return 0;
}