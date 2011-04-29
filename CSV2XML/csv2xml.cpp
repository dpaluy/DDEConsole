/* $Id: csv2xml.cpp,v 1.11 2005/07/18 07:36:54 jacob Exp $
 *
 * csv2xml - A simple csv to xml converter
 *
 * Origionally developed by Jacob Rhoden, for
 * usage in FreeBSD. This program is released
 * under the open source liscence.
 *
 *  Please email/report any bugs to:
 *      url: http://rhoden.id.au/contact.html
 *    email: jacob@rhoden.id.au
 */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#define MAX_TOKEN 2000
#define MAX_FIELDS 200

int line;
int fieldcount;
char *fields[MAX_FIELDS];
int xmlmode;
char sub_char = '\0';
int nullify;
char *encoding;

void cleanup(void) {
  int i;

  for(i=0;i<fieldcount;i++) {
    delete fields[i];
    fields[i]=NULL;
    }

  }

void warn(char *message) {

  fprintf(stderr,"Warning: %s on line %d\n",message,line);

  }

int fatal(char *message) {

  fprintf(stderr,"Error: %s on line %d\n",message,line);
  cleanup();
  exit(1);

  }

int get_csv_token(char *token, int maxlen) {
  int c;
  int quoted;
  int len;

  len=0;
  quoted=0;
  while((c=getchar()) && len<maxlen) {
    if(c==-1) { break; }
    if(c=='"' && quoted==1) {
      c=getchar();
      if(c!='"') { quoted=0; }
      }
    if(len==0 && c=='"' && quoted==0) { quoted=1; continue; }
    if(quoted==0 && c==',') { *token='\0'; return(1); }
    if(c==10) { line++; }
    if(quoted==0 && c==10) { *token='\0'; return(0); }
    *token=c;
    len++;
    token++;
    }
  if(len==maxlen) { fatal("Field was too long for this program to handle"); }

  *token='\0';
  return(-1);
  }

/*
 * If a field header is to be used as a xml token, we
 * must remove special characters and so fourth
 */
void sanitize(char *token) {

  while(*token!='\0') {
    if(
       (*token>='0' && *token<='9') ||
       (*token>='a' && *token<='z') ||
       (*token>='A' && *token<='Z')
      ) {
      *token++;
      }
    else {
      *token='_';
      *token++;
      }
    }

  }

/*
 * This function is called to read the first row and store
 * the field names as the header names.
 */
int get_field_headers() {
  char token[MAX_TOKEN+1];
  int status;

  status=1;
  fieldcount=0;
  memset(&fields,0,sizeof(fields));
  while(fieldcount<MAX_FIELDS && status>0) {
    status=get_csv_token((char*)&token,MAX_TOKEN);
    if(status<0) { break; }
    if(token[0]==0) { fatal("Header row has null string"); }
    if(xmlmode!=2) { sanitize((char*)&token); }
    fields[fieldcount]=new char[strlen(token)+1];
    strcpy(fields[fieldcount],token);
    fieldcount++;
    }
    if(fieldcount==MAX_FIELDS) { fatal("Too many columns in this csv file"); }

  return(status);
  }

void print_entity(char *e) {

  /*
   * printing a string character by character is not the best way to do this
   * but it works for the time being.
   */
  while(*e!='\0') {
    if(*e=='&') { printf("&amp;"); }
    else if(*e=='<') { printf("&lt;"); }
    else if(*e=='>') { printf("&gt;"); }
    else if(*e=='"') { printf("&quot;"); }
    else if(*e=='\'') { printf("&apos;"); }
    else if(*e<0 && sub_char!='\0') { printf("%c", sub_char); }
    else printf("%c",*e);
    e++;
    }

  }

int get_row() {
  char *crow[MAX_FIELDS];
  char token[MAX_TOKEN+1];
  int status;
  int colcount;
  int i;

  colcount=0;
  status=1;
  memset(&crow,0,sizeof(crow));
  while(colcount<MAX_FIELDS && status>0) {
    status=get_csv_token((char*)&token,MAX_TOKEN);
    if(status<0) { break; }
    crow[colcount]=new char[strlen(token)+1];
    strcpy(crow[colcount],token);
    colcount++;
    }

  if(colcount==0) {
    cleanup();
    exit(0);
    }
  if(colcount==MAX_FIELDS) {
    fatal("Row contained more fields than this sofware can handle");
    }
  if(colcount!=fieldcount) {
    warn("Row count does not match field count");
    return(status);
    }

  printf("<row>\n");
  for(i=0;i<colcount;i++) {
    if(nullify==0 || crow[i][0]!='\0') {
      if(xmlmode==1) {
        printf("  <%s value=\"",fields[i]);
        print_entity(crow[i]);
        printf("\" />\n");
        }
      else if(xmlmode==2) {
        printf("  <item name=\"%s\">",fields[i]);
        print_entity(crow[i]);
        printf("</item>\n");
        }
      else {
        printf("  <%s>",fields[i]);
        print_entity(crow[i]);
        printf("</%s>\n",fields[i]);
        }
      }
    delete crow[i];
    crow[i]=NULL;
    }
  printf("</row>\n");


  return(status);
  }


void version_help(void) {

  fprintf(stderr,"csv2xml 0.6 - Open source csv to xml converter\n");
  fprintf(stderr,"\n");
  fprintf(stderr,"Version: 0.6\n");
  fprintf(stderr,"Date: July 15, 2005\n");
  fprintf(stderr,"\n");

  exit(0);
  }

void option_help(void) {

  fprintf(stderr,"csv2xml 0.6 - Open source csv to xml converter\n");
  fprintf(stderr,"\n");
  fprintf(stderr,"General usage:  csv2xml < infile > outfile\n");
  fprintf(stderr,"\n");
//  fprintf(stderr," -e=ENCODING  Set encoding, default is utf-8\n");
  fprintf(stderr," -m=number    Set xml mode. 0-2\n");
  fprintf(stderr," -s=char      To strip unicode characters, specify a replacement character.");
  fprintf(stderr," -n           Hide fields that are null/empty\n");
  fprintf(stderr," -h           Output this help.\n");
  fprintf(stderr," -v           Display version information.\n");
  fprintf(stderr,"\n");

  exit(1);
  }

void opt_xmlmode(char *opt) {

  if(opt[2]!='=') { option_help(); }
  if(opt[3]<'0' || opt[3]>'9') { option_help(); }
  if(opt[4]!='\0') { option_help(); }
  xmlmode=(int)opt[3]-'0';
  if(xmlmode>2) { option_help(); }

  }

void opt_encoding(char *opt) {

  if(opt[2]!='=') { option_help(); }

  }

void opt_sub_non_unicode(char *opt) {
  
    if(opt[2]!='=' || opt[3]=='\0') { option_help(); }
    sub_char = opt[3];
}
    

void get_options(int argc,char *argv[]) {
  int i;

  xmlmode=0;
  encoding=NULL;

  for(i=1;i<argc;i++) {
    if(argv[i][0]!='-') { option_help(); }
    if(argv[i][1]=='v') { version_help(); }
    else if(argv[i][1]=='h') { option_help(); }
    else if(argv[i][1]=='n') { nullify=1; }
    else if(argv[i][1]=='m') { opt_xmlmode(argv[i]); }
//    else if(argv[i][1]=='e') { opt_encoding(argv[i]); }
    else if(argv[i][1]=='s') { opt_sub_non_unicode(argv[i]); }
    else { option_help(); }
    }

  }

int main(int argc,char *argv[]) {
  int status;

  get_options(argc,argv);

  fieldcount=0;
  status=get_field_headers();

  if(status<0) {
    fatal("Header row contains no entities.");
    }

  line=1;
  while(get_row()>=0);

  cleanup();
  exit(0);
  }
