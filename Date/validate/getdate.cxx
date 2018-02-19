#include <iostream>

struct GetDate
{
  int Month()
  {
   if(timeInfo == NULL)return -1;
   else
   return timeInfo->tm_mon+1;
  }

  int Year()
  {
   if(timeInfo == NULL)return -1;
   else
   return timeInfo->tm_year+1900;
  }
 
  int Day()
  {
   if(timeInfo == NULL)return -1;
   else
   return timeInfo->tm_mday;
  }
  
  private:
  time_t timeval = time(NULL);
  tm *timeInfo = localtime(&timeval);
}get;


int main()
{
 int day;
 int month;
 int year;
 
 day = get.Day();
 month = get.Month();
 year = get.Year();
 
 std::cout<<"\n\t"<<day<<"/"<<month<<"/"<<year<<"\n\n";
 return 0;
}