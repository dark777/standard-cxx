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
                             "((?:[á-úA-Za-z]+[[:space:]]+[á-úA-Za-z]{0,20})?)?" //valida nome e sobrenome
                             "((?:[[:space:]]+[á-úA-Za-z]{0,10})?)?" // valida nome, nome composto e sobrenome
                            );
    return std::regex_match(_nome, name_smatch, pattern);
   }
   
   void print()
   {
    std::cout<<"\n\tNome: "<<_nome<<(isName()?" is Valid\n":" is Invalid\n");
   }
   
   private:
    std::string _nome;
};


int main()
{    
 std::string strname;
    
 do{
    std::cout << "\n\tEnter full name: ";
    getline(std::cin, strname);
    
    name(strname).print();
    std::cout<<"\n";
    
   }while(!name(strname).isName());
  
  return 0;
}