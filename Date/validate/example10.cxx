#include <iostream>

std::string current_date(const std::string& format)
{
 tm dtm;
 
 time_t timer = time(NULL);
 
 char temp[32] = "";
 
 if(format == "yyyy-mm-dd")
  {
   strftime(temp, sizeof(temp) - 1, "%Y-%m-%d", localtime_r(&timer, &dtm));
  }
 else 
 if(format == "yy-mm-dd")
  {
   strftime(temp, sizeof(temp) - 1, "%y-%m-%d", localtime_r(&timer, &dtm));
  }
 else 
 if(format == "yyyymmdd")
  {
   strftime(temp, sizeof(temp) - 1, "%Y%m%d", localtime_r(&timer, &dtm));
  }
 else 
 if(format == "yymmdd")
  {
   strftime(temp, sizeof(temp) - 1, "%y%m%d", localtime_r(&timer, &dtm));
  }
  
 return temp;
}

std::string current_time(const std::string& format)
{
 tm dtm;
 
 time_t timer = time(NULL);
 
 char temp[32] = "";
 
 if(format == "HH:MM:SS")
  {
   strftime(temp, sizeof(temp) - 1, "%H:%M:%S", localtime_r(&timer, &dtm));
  }
 else
 if(format == "HH:MM")
  {
   strftime (temp, sizeof(temp) - 1, "%H:%M", localtime_r(&timer, &dtm));
  }
 else
 if(format == "HHMMSS")
  {
   strftime(temp, sizeof(temp) - 1, "%H%M%S", localtime_r(&timer, &dtm));
  }
 else
 if(format == "HHMM")
  {
   strftime(temp, sizeof(temp) - 1, "%H%M", localtime_r(&timer, &dtm));
  }
 return temp;
}

std::string exp_date(int days, int hours, int minutes, int seconds)
{
 tm dtm;
 
 time_t t = time(NULL) + seconds + 60 * (minutes + 60 * (hours + 24 * days));
 
 char date[256] = "";
 
 strftime(date, sizeof(date), "%A, %d-%b-%Y %H:%M:%S GMT", gmtime_r(&t, &dtm));
 
 return date;
} 

int main()
{
 std::cout<<"\n\t"<<current_date("yyyy-mm-dd")<<" "<<current_time("HH:MM:SS")<<"\n\t"<<exp_date(34,12,35,59)<<"\n\n";
 
 return 0;
}