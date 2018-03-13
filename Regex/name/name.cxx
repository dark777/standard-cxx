#include <regex>
#include <iostream>

struct name
{  
 name(std::string nome): _name(nome){}
   
 ~name()
  {
   if(_name.length() != 0)_name.clear();    
  }
   
 bool isName()
 {
  std::smatch name_smatch;
    
  const std::regex pattern(
                           "(?:[á-úA-Za-z]+[[:space:]]+[á-úA-Za-z.]{0,20})?" //valida nome e sobrenome
                           "(?:[[:space:]]+[á-úA-Za-z.]{0,10})?" // valida nome, nome composto e sobrenome
                           "(?:[[:space:]]+[á-úA-Za-z.]{0,10})?" // valida nome, 2 nomes composto e sobrenome
                           "(?:[[:space:]]+[á-úA-Za-z.]{0,10})?" // valida nome, 3 nomes composto e sobrenome
                          );
  
  return std::regex_match(_name, name_smatch, pattern);
 }
 
 void print()
 {
  std::cout << "\n\tName: "+_name+(isName()?" is Valid\n":" is Invalid\n");
 }
   
 private:
  std::string _name;
};

int main()
{
 std::string _nome; 
 do{
    std::cout << "\n\tEnter full name: ";
    getline(std::cin, _nome);
       
    name(_nome).print();
    std::cout << "\n";
       
   }while(!name(_nome).isName());
 
 return 0;
}