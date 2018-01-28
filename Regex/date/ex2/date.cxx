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
  //regex valida datas a partir de 1904 e anos bisextos nos formatos 0000-string-00 ou 0000-00-00 | 0000.string.00 ou 0000.00.00 | 0000/string/00 ou 0000/00/00
  const std::regex pattern("^(?:\\d{4}[.-/](?:(?:(?:(?:0[13578]|1[02]|Jan|Mar|May|Jul|Aug|Oct|Dec)[.-/](?:0[1-9]|1[0-9]|2[0-9]|3[0|1]))|(?:(?:0[469]|11|Apr|Jun|S‌​ep|Nov)[.-/](?:0[1-9]|[1-2][0-9]|30))|(?:0[2]|Feb[.-/](?:0[1-9]|1[0-9]|2[0-8]))))|(?:(?:\\d{2}(?:0[48]|[2468][04‌​8]|[13579][26]))|(?:‌​(?:[02468][048])|[13‌​579][26])00)[.-/]Feb[.-/]29)$");
   
  return std::regex_match(this._date, date_smatch, pattern);
 }
 
 date* print()
 {
  std::cout<<"\n\tEmail: "<<this._date<<(date(this._date).isDate()?" is Valid\n":" is Invalid\n"); 
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
    
   }while(date(str).isDate() == 0);
  
  return 0;
}