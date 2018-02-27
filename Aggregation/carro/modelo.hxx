#ifndef MODELO_HXX
#define MODELO_HXX

struct Marca;

struct Modelo
{
 Modelo(){}
  ~Modelo(){} 
  int idModelo;
  std::string descriModelo;
 Marca marca;
};

#endif