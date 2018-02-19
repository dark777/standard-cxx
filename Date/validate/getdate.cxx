#include <iostream>

struct GetDate
{
  int Month()
  {
   if(&timeInfo == NULL)return -1;
   else
   return timeInfo.tm_mon+1;
  }

  int Year()
  {
   if(&timeInfo == NULL)return -1;
   else
   return timeInfo.tm_year+1900;
  }
 
  int Day()
  {
   if(&timeInfo == NULL)return -1;
   else
   return timeInfo.tm_mday;
  }
   
  int dayOfWeek(int day, int month, int year)
   {
    timeInfo.tm_year = year - 1900;
    timeInfo.tm_mon = month - 1;
    timeInfo.tm_mday = day;
    timeInfo.tm_hour = 12;
    mktime(&timeInfo);
    return timeInfo.tm_wday;
   }
  
  std::string diaSemana()
  {
   //Algoritmo para descobrir o dia da semana
   int a = ((12 - Month()) / 10);
   int b = (Year() - a);
   int c = (Month() + (12 * a));
   int d = (b/100);
   int e = (d/4);
   int f = ((2-d) + e);
   int g = int(365.25 * b);
   int h = int(30.6001 * (c + 1));
   int i = int((f + g) + (h + Day()) + 5);
   int j = int(i % 7); //Resto de I por 7, onde 0=Saturday, 1=Sunday, 2=Monday, 3=Tuesday, 4=Wednesday, 5=Thursday, 6=Friday

   //Testa o resultado e retorna
   switch(j)
    {
     case 0: return "Saturday";
     case 1: return "Sunday";
     case 2: return "Monday";
     case 3: return "Tuesday";
     case 4: return "Wednesday";
     case 5: return "Thursday";
     case 6: return "Friday";
    }
  }
  
  private:
  time_t timeval = time(NULL);
  tm timeInfo = *localtime(&timeval);
}get;


int main()
{
 int day;
 int month;
 int year;
 
 day = get.Day();
 month = get.Month();
 year = get.Year();
 
 std::cout<<"\n\tweekday: "<<get.diaSemana()<<" "<<day<<"/"<<month<<"/"<<year<<"\n\n";
 return 0;
}