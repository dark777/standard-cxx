#!/usr/bin/perl
# Acima deve ser colocado o caminho do Perl

################################################################
# CONSULTE O ARQUIVO MANUAL.HTML PARA                          #
# SABER COMO CONFIGURAR ESSE CGI.                              #
# A versao 2.5 esta mais segura e mais objetiva                #
# ------------------------------------                         #
# Programa FormUla v2.5                                        #
# Data 17/09/2002                                              #
# Arquivos: formula.cgi, formulario.html, formulario2.html     # 
#           erro.html, confirmado.html, manual.html, botao.gif #
#                                                              #
# http://www.webscriptonline.kit.net                           #
# ------------------------------------                         #
################################################################



############### CONFIGURACAO ###################################

# Coloque abaixo o seu e-mail que vai receber os campos do formulario
# Existe possibilidade de colocar uma \ (barra) antes da @ (arroba). Exemplo:  \@
$SeuEmail = 'seuemail@webscript.kit.net';

# Modifique abaixo com o seu dominio ou os que podem ter acesso ao script
@referers = ('www.webscriptonline.kit.net','webscript.kit.net');

# Caminho do sendmail em seu servidor
$sendmail = '/usr/sbin/sendmail';

# Pagina de erro
$erro = "http://www.webscriptonline.kit.net/erro.html";

# Pagina de confirmacao de envio do formulario
$fim = "http://www.webscriptonline.kit.net/confirmado.html";

# Coloque abaixo o que voce quer que apareca no assunto
$Subject = "Contato!";


################ FIM DA CONFIGURACAO ############################



#############################################
######## NAO ALTERE NADA ABAIXO #############
#############################################

&segurovamp;
&metodo;
$Nome = $in{'Nome'};
$Email = $in{'Email'};
$Telefone = $in{'Telefone'};
$Mensagem = $in{'Mensagem'};
&formatoemail;
&checa;
&form;
print "Location: $fim\n\n";
exit;

#######################################

sub form {
open (MAIL,"|$sendmail -t");
print MAIL "To: $SeuEmail\n";
print MAIL "From: $Email\n";
print MAIL "Subject: $Subject\n";
print MAIL "$Mensagem\n\n";
print MAIL "$Nome\n";
print MAIL "E-mail: $Email\n\n";
print MAIL "Telefone: $Telefone\n\n";
close (MAIL);
}

#######################################

sub metodo { local (*in) = @_ if @_; 
local ($i, $key, $val); if ( $ENV{'REQUEST_METHOD'} eq "GET" ) 
{$in = $ENV{'QUERY_STRING'};} 
elsif ($ENV{'REQUEST_METHOD'} eq "POST") 
{read(STDIN,$in,$ENV{'CONTENT_LENGTH'});} 
else { 
$in = ( grep( !/^-/, @ARGV )) [0];
$in =~ s/\\&/&/g; } @in = split(/&/,$in);
foreach $i (0 .. $#in) { 
$in[$i] =~ s/\+/ /g; 
($key, $val) = split(/=/,$in[$i],2); 
$key =~ s/%(..)/pack("c",hex($1))/ge; 
$val =~ s/%(..)/pack("c",hex($1))/ge; 
$in{$key} .= "\0" if (defined($in{$key})); 
$in{$key} .= $val; } return length($in); }

#######################################

sub formatoemail {
if (index($Email, "@") < 1)  {&esquec;}
if (index($Email, ".") < 1)  {&esquec;}
if (index($Email, " ") > -1) {&esquec;}
}
sub checa {
if (!$Nome || $Nome eq ' ') {&esquec;}
if (!$Email || $Email eq ' ') {&esquec;}
if (!$Telefone || $Telefone eq ' ') {&esquec;}
if (!$Mensagem || $Mensagem eq ' ') {&esquec;}

}
sub esquec {
print "Location: $erro\n\n";
exit;
}

#######################################
sub segurovamp {
if ($ENV{'HTTP_REFERER'}) {
foreach $referer (@referers) {
if ($ENV{'HTTP_REFERER'} =~ /$referer/i) {
$check_referer = '1';
last;
}}}
else {$check_referer = '1';}
if ($check_referer != 1) {
print "Location: $erro\n\n";
exit;
}}

#######################################
exit;

###visite www.webscriptonline.kit.net### - construção e desenvolvimento de home pages.