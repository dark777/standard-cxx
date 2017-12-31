#include <iostream>
 
struct Bicicleta
{
  std::string modelo;
  std::string quadro;
  std::string freios;
  std::string rodas;
 
  // Construtor Default
  Bicicleta(){}
 
  // Construtor Override
  Bicicleta(std::string bmodelo, std::string bquadro, std::string bfreios, std::string brodas): 
  modelo(bmodelo),quadro(bquadro),rodas(brodas),freios(bfreios){}
 
  //Metodo Override 
  void bicicleta()
  {
   std::cout << "\n\tBicicleta.: " + modelo
             << "\n\tQuadro....: " + quadro
             << "\n\tRodas.....: " + rodas
             << "\n\tFreios....: " + freios
             << "\n\n";
  }
};
 
struct BicicletaMontanha: Bicicleta
 {
   BicicletaMontanha(){} 
   std::string marchas;
   std::string amortecedores;
   std::string garrafa;
   std::string gps;
 
 //Metodo Override
  void bicicleta()
  {
   std::cout << "\n\tBicicleta.....: " + modelo
             << "\n\tQuadro........: " + quadro
             << "\n\tRodas.........: " + rodas
             << "\n\tFreios........: " + freios
             << "\n\tMarchas.......: " + marchas
             << "\n\tAmortecedores.: " + freios
             << "\n\tGarrafa.......: " + garrafa
             << "\n\tGps...........: " + gps
             << "\n\n";     
  }
};