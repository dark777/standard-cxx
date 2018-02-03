#include <regex>
#include <iostream>

#define this (*this)

struct phone
{ 
 phone(std::string tel): _tel(tel){}
 
 ~phone()
  {
   if(_tel.length() != 0)_tel.clear();
  }
  /*
   * ^(\\(?[0-9]{2}\\)?|[-. ]?)[-. ]?[0-9]{4}[-. ]?[0-9]{4}$
   * valida nos formatos:
   * xx xxxx xxxx, xx.xxxx.xxxx, xx-xxxx-xxxx
   * (xx) xxxx xxx, (xx).xxxx.xxx,(xx)-xxxx-xxx
   * xxxx xxxx, xxxx.xxxx, xxxx-xxxx
   * 
   * teste:
   * ^((\(\d{2}\))|\d{2}) ?9?\d{4}-?\d{4}$
   * o ^ no início significa que a expressão precisa obrigatoriamente começar por ali 
   * e o $ significa que precisa obrigatoriamente terminar ali.
   * https://pt.stackoverflow.com/questions/166484/validar-n%C3%BAmero-de-telefone-com-nono-d%C3%ADgito-opcional
   */
 
 bool isPhone()
 {
  std::smatch phone_smatch;
  
  const std::regex pattern("^(\\([1-9]{2}\\) 9?[0-9]{4}[-.][0-9]{4})$");
  
  return std::regex_match(this._tel, phone_smatch, pattern);
 }
 
 phone* print()
 {
  std::cout<<"\n\tPhone: "<<this._tel<<(phone(this._tel).isPhone()?" is Valid\n":" is Invalid\n");
 }
 
 private:
 std::string _tel;
};


int main(void)
{
  std::string strphone;
  
  do{
    
     std::cout << "\n\tEnter fone mask(00) 00000-0000: ";
     getline(std::cin, strphone);
    
     phone(strphone).print();
     std::cout<<"\n";
    
    }while(phone(strphone).isPhone() == 0);
  
  return 0;
}