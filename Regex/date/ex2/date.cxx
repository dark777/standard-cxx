#include <regex>
#include <iostream>

#define this (*this)

struct date
{
 date(std::string mail): _date(mail){}
 
 ~date()
  {
   if(_date.length() != 0) _date.clear();
  }
 
 bool isDate()
 {
  std::smatch date_smatch;
  
  //regex valida datas e anos bisextos nos formatos yyyy/[A|a]/dd ou yyyy/m/dd | yyyy.[A|a].dd ou yyyy.m.dd | yyyy-[A|a]-dd ou yyyy-m-dd
  //const std::regex pattern("^(?:\\d{4}(?:/|.|-)(?:(?:(?:(?:0[13578]|1[02]|(?:jan|mar|may|jul|aug|oct|dec|Jan|Mar|May|Jul|Aug|Oct|Dec))(?:/|.|-)(?:0[1-9]|[1-2][0-9]|3[01]))|(?:(?:0[469]|11|(?:apr|jun|sep|nov|Apr|Jun|S‌​ep|Nov))(?:/|.|-)(?:0[1-9]|[1-2][0-9]|30))|(?:(?:02|feb|Feb)(?:/|.|-)(?:0[1-9]|1[0-9]|2[0-8]))))|(?:(?:\\d{2}(?:0[48]|[2468][048]|[13579][26]))|(?:(?:[02468][048])|[13579][26])00)(?:/|.|-)(?:02|feb|Feb)(?:/|.|-)29)$");
  
  //regex valida datas e anos bisextos nos formatos dd/mm/yyyy ou dd/[A|a]/yyyy
  const std::regex pattern("^(?:(?:0[1-9]|1[0-9]|2[0-8])(?:/|.|-)(?:0[1-9]|1[0-2]|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)|(?:(?:29|30)(?:/|.|-)(?:0[13456789]|1[0-2]|jan|mar|apr|may|jun|jul|sep|oct|nov|dec|Jan|Mar|Apr|May|Jun|Jul|Sep|Oct|Nov|Dec))|(?:31(?:/|.|-)(?:0[13578]|1[02]|jan|mar|may|jul|aug|oct|dec|Jan|Mar|May|Jul|Aug|Oct|Dec)))(?:/|.|-)(?:[2-9][0-9]{3}|1[6-9][0-9]{2}|159[0-9]|158[3-9])|29(?:/|.|-)(?:02|feb|Feb)(?:/|.|-)(?:(?:[2-9](?:04|08|[2468][048]|[13579][26])|1[6-9](?:(?:04|08|[2468][048]|[13579][26])00)|159(?:2|6)|158(?:4|8))|(?:16|[2468][048]|[3579][26])00)$");
  return std::regex_match(this._date, date_smatch, pattern);
 }
 
 date& print()
 {
  std::cout<<"\n\tDate: "<<this._date<<(date(this._date).isDate()?" is Valid\n":" is Invalid\n");
  return this;
 }
 
 private:
  std::string _date;
};

int main()
{    
 std::string str;
    
 do{
    
    std::cout << "\n\tEnter date: ";
    getline(std::cin, str);
    
    date(str).print();
    std::cout<<"\n";
    
   }while(date(str).isDate() == 1);
  
  return 0;
}