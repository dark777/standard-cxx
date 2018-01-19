#include <cstdio>

struct No
{
 int verticeNo; //vertice
 No *prox;
};

struct Grafo
{
 int verticeGr;  //vertice
 No *adj;
 Grafo *prox;
};

No *criaNo(int v1, No *p)
{
 No *x = new No();
 
 x->verticeNo = v1;
 x->prox = p;
 return x;
}

Grafo *criaGrafoNo(int v1, No *p, Grafo *g)
{
 Grafo *x = new Grafo();
 
 x->verticeGr = v1;
 x->adj = p;
 x->prox = g;
 return x;
}

void inserirAdj(Grafo *g)
{
 No *l = NULL;
 int i;
 int quant;
 int vertice; //vertice
 
 while (g != NULL)
 {
  printf("Informe o numero de ligacoes partindo da cidade %d: ",g->verticeGr);
  scanf("%d",&quant);
  
  for(i=0;i<quant;i++)
  {
   printf("   ------ > Codigo da cidade de chegada: ");
   scanf("%d",&vertice);
   l=criaNo(vertice,l);
  }
  g->adj=l;
  g=g->prox;
 }
}

void percorrerListaAdj(Grafo *g)
{
 while(g != NULL)
  {
   printf("\n\n      Codigo cidade: %d",g->verticeGr);
   
   while(g->adj != NULL)
    {
     printf("\n  De cidade %d para %d ",g->verticeGr,g->adj->verticeNo);
     g->adj = g->adj->prox;
    }
   g=g->prox;
  }
}

int main()
{
 int i;
 int quant;
 int vertice; //vertice
 
 Grafo *g = NULL;
 
 printf("\n\tInsira a quantidade de cidades (vertices): ");
 scanf("%d",&quant);
 
 for(i=0;i<quant;i++)
 {
  printf("\n\tInforme o codigo da cidade: ");
  scanf("%d",&vertice);
  g=criaGrafoNo(vertice,NULL,g);
 }
 
 inserirAdj(g);
 percorrerListaAdj(g);
 
 return 0;
}