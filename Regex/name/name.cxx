#include <regex>
#include <iostream>

struct name
{  
 name(std::string nome): _nome(nome){}
   
 ~name()
  {
   if(_nome.length() != 0)_nome.clear();    
  }
   
 bool isName()
 {
  std::smatch name_smatch;
    
  const std::regex pattern(
                           "((?:[á-úA-Za-z]+[[:space:]]+[á-úA-Za-z.]{0,20})?)?" //valida nome e sobrenome
                           "((?:[[:space:]]+[á-úA-Za-z.]{0,10})?)?" // valida nome, nome composto e sobrenome
                           "((?:[[:space:]]+[á-úA-Za-z.]{0,10})?)?" // valida nome, 2 nomes composto e sobrenome
                           "((?:[[:space:]]+[á-úA-Za-z.]{0,10})?)?" // valida nome, 3 nomes composto e sobrenome
                          );
  
  return std::regex_match(_nome, name_smatch, pattern);
 }
 
 std::string print()
 {
  return "\n\tName: "+_nome+(isName()?" is Valid\n":" is Invalid\n");
 }
   
 private:
  std::string _nome;
};

int main()
{
 do{
    std::cout << "\n\tEnter full name: ";
    getline(std::cin, _nome);
       
    std::cout << name(_nome).print() << "\n";
       
   }while(!name(_nome).isName());
 
 return 0;
}