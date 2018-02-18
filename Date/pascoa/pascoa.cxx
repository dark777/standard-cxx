#include <iostream>
/*
.--------.-------------------------------.----------------------------------.
|Formato |Britânico:                     |Americano:                        |
|        |Dia - Mês - Ano                |Mês - Dia - Ano                   |
|--------+-------------------------------+----------------------------------|
|    A   | the Twenty-fourth of February,| February, the Twenty-fourth,2009 |
|        |             2009              |                                  |
|--------+-------------------------------+----------------------------------|
|    B   |     24th February 2009        |     February 24th, 2009          |
|    C   |      24 February 2009         |      February 24, 2009           |
|    D   |          24/2/2009            |          2/24/2009               |
|    E   |           24/2/09             |           2/24/09                |
|    F   |          24/02/09             |          02/24/09                |
'--------'-------------------------------'----------------------------------'
*/

int main()
{
  time_t now = time(0);
  tm *ltm = localtime(&now);
  
  int current_day(ltm->tm_mday);
  int current_month(ltm->tm_mon+1);
  int current_year(ltm->tm_year+1900);
  
  printf("\n\tThe current year is %4d\n",current_year);
  
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
 
 if(year == current_year && current_month == 4 && current_day == day_easter)
 printf("\n\tToday %dth %s %04d is Easter Day",day_easter, months[month-1], year); //British date format
 else
 if(year >= current_year && current_month <= 4 && (current_day < day_easter || current_day > day_easter))
 printf("\n\tEaster will fall. !!\n\t%s %dth, %04d\n\n", months[month-1], day_easter, year); //American date format
 else
 printf("\n\tEaster has fallen. !!\n\t%dth %s %04d\n\n", day_easter, months[month-1], year); //British date format
 
 return 0;
}