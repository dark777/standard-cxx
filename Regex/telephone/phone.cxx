#include <regex>
#include <iostream>

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
  
  //const std::regex pattern("^(\\([1-9]{2}\\) 9?[0-9]{4}[-.][0-9]{4})$");
  
  //valida todas as areas (?:(1[1-9])|(2[12478])|(3[1-8])|(5[1345])|(6[1-9])|(7[134579])|(8[1-7])|(9[1-5])|49)
  const std::regex pattern("(^\\((?:([1356789][1345])|([123689]2)|([167][79])|([16][68])|([23][78])|(8[67])|(2[14])|49)\\)[[:space:]]9?[0-9]{4}[-.][0-9]{4})$");
  return std::regex_match(_tel, phone_smatch, pattern);
 }
 
 std::string print()
 {
  return "\n\tPhone: "+_tel+(isPhone()?" is Valid\n":" is Invalid\n");
 }
 
 private:
  std::string _tel;
};

int main()
{
  std::string strphone;
  
  do{
         
     std::cout << "\n\tEnter fone mask(00) 00000-0000: ";
     getline(std::cin, strphone);
     
     std::cout << phone(strphone).print() << "\n";
     
    }while(!phone(strphone).isPhone());
    
  return 0;
}