#include <iostream>
//Julian Date
//https://calendar.zoznam.sk/julian_calendar-sp.php?ly=1987#March
int GetDates(int day, int month, int year); // Function prototype.
int JulianDateNum(int day,int jdn,long intRes1,long intRes2,long intRes3);

int main()
{
 int month; //Declare variables
 int day;
 int year;
 int jdn=0;
 int dayOfWeek=0;
 
 do{
    std::cout << "\n\tPlease enter the day (dd): "; // Gets day input from user.
    std::cin >> day;
    
    if(day < 01 || day > 31)
    std::cout << "\n\tInvalid data! Please enter a day "
              << "less from 1-31: "; // Checks for invalid input.
      
   }while(day < 01 || day > 31);
   
 do{
    std::cout << "\n\tPlease enter the month (mm): "; // Gets month input from user.
    std::cin >> month;
    
    if(month < 01 || month > 12)
    std::cout << "\n\tInvalid data! Please enter a month "
              << "from 01-12: "; // Checks for invalid input.
         
   }while(month < 01 || month > 12);
   
  std::cout << "\n\tPlease enter the year (yyyy): "; // Gets year input from user.
  std::cin >> year;
  
  jdn=GetDates(day,month,year);
   
  dayOfWeek=((jdn + 1)%7); // calculates day of the week
  
  switch(dayOfWeek)
   {
    case 0:
    std::cout << "\n\tMonday";
    break;
    
    case 1:
    std::cout << "\n\tTuesday";    
    break;
    
    case 2:
    std::cout << "\n\tWednesday";
    break;
    
    case 3:
    std::cout << "\n\tThursday";
    break;
    
    case 4:
    std::cout << "\n\tFriday";
    break;
    
    case 5:
    std::cout << "\n\tSaturday";
    break;
    
    case 6:
    std::cout << "\n\tSunday";
    break;
    
    default:
    std::cout<<"\n\tDia invalido ..!!!\n\n";
   }

   std::cout << "\n\tThe Day of the week for " << day << "/" // Outputs day of week results.
             << month << "/" << year << " is: " << dayOfWeek
             << "\n\n";
  return 0; // Terminates program.
}

int GetDates(int day, int month, int year) // Function that calculates the julian date.
{
 long intRes1((2 - year / 100) + (year / 400)); // Calculation formula.
 long intRes2((int)(365.25 * year));
 long intRes3((int)(30.6001 * (month + 1)));

 int jdn(intRes1 + intRes2 + intRes3 + day + 1720994.5);

 return jdn;
}