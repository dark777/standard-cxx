#include "libmenu.hxx"


































int main()
{
 int op;
 int tam=4;
 std::string opcoes[tam]={"Novo","Consultar","Listar","Sair"};
 //menu(tam, opcoes); // aceita até 15 opçoes
 menu(tam, tam, opcoes); // aceita limite igual ao tamanho do vetor
 std::cin>>op;
 return 0;
}