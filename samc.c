#include <stdio.h>
#include <stdlib.h>
#include <limits.h>
#include <errno.h>
#include <string.h>

#define MAX_WORD_LEN 200
#define STACK_SIZE 1000
#define MAX_STRINGS 4000
#define MAX_CONSTS 4000
#define MAX_WORDS 4000

char word[MAX_WORD_LEN];
int labels=0;
int state=0;
int base=10;

int psp_index=0;
int psp[STACK_SIZE];
#define PSP_POP() psp[--psp_index]
#define PSP_PUSH(val) psp[psp_index++]=(val)

int rsp_index=0;
int rsp[STACK_SIZE];
#define RSP_POP() rsp[--rsp_index]
#define RSP_PUSH(val) rsp[rsp_index++]=(val)

char *strings[MAX_STRINGS];
int strings_len=0;

char consts[MAX_CONSTS][MAX_WORD_LEN];
int consts_value[MAX_CONSTS];
int consts_len=0;

char words[MAX_WORDS][MAX_WORD_LEN];
char words_label[MAX_WORDS][MAX_WORD_LEN];
int words_len=0;

void compile(FILE *in);

int is_space(int c)
{
  return c == ' ' || c == '\t' || c == 13 || c == 10;
}

void translate()
{
  for(int i=0; i<words_len; i++)
    if(!strcmp(words[i], word))
      strcpy(word, words_label[i]);
}

int next_word(FILE *in)
{
  char *w=word;
  int c;
  
  do
    {
      c = getc(in);
      if(c == EOF)
        return 0;
    }
  while(is_space(c));

  // skip comments
  if(c=='\\')
    {
      while (c!=13 && c!=10)
        {
          c=getc(in);
          if(c==EOF)
            return 0;
        }
      return next_word(in);
    }

  // read word
  *w++=c;
  while(1)
    {
      c = getc(in);
      if(c == EOF)
        return 0;
      
      if(is_space(c))
        break;

      *w++=c;
    }
  *w++=0;

  translate();

  return 1;
}

void error_unknown_word()
{
  fprintf(stderr, "Unknown word: `%s'\n", word);
  exit(1);
}

void error_eof()
{
  fprintf(stderr, "Unexpected end of file\n");
  exit(1);
}

void error_overflow()
{
  fprintf(stderr, "Overflow `%s'\n", word);
  exit(1);
}

void error_file_not_found(const char *filename)
{
  fprintf(stderr, "File `%s' not found\n", filename);
  exit(1);
}

int is_number()
{
  char *endptr;
  long value = strtol(word, &endptr, base);
  
  if((errno==ERANGE && (value==LONG_MAX || value==LONG_MIN))
     || errno!=0 && value==0
     || value<INT_MIN || value>INT_MAX
     || *endptr != 0)
    return 0;
  else
    return 1;
}

int to_number()
{
  long value = strtol(word, NULL, base);
  
  if((errno==ERANGE && (value==LONG_MAX || value==LONG_MIN))
     || errno!=0 && value==0
     || value<INT_MIN || value>INT_MAX)
    error_overflow();
  
  return value;
}

int is_immediate()
{
  if(!strcmp(word, "HEX")
     || !strcmp(word, "DEC")
     || !strcmp(word, "BIN")
     || !strcmp(word, "("))
    return 1;
  else
    return 0;
}

void set_label(const char *word, const char *label)
{
  strcpy(words[words_len], word);
  strcpy(words_label[words_len++], label);
}

void create(FILE *in)
{
  if(!next_word(in))
    error_eof();
  
  char label[256];
  if(strcmp(word, "MAIN"))
    {
      sprintf(label, "label%d", labels++);
      set_label(word, label);
    }
  else
    strcpy(label, "MAIN");

  printf("\n\t; %s\n%s:\n", word, label);
  printf("\tcall DOCOLON\n.begin:\n");
}

void eexit()
{
  printf("\tdw EXIT\n");
}

void lit(int value)
{
  printf("\tdw LIT, %d\n", value);
}

int exists_const()
{
  for(int i=0; i<consts_len; i++)
    if(!strcmp(consts[i], word))
      return 1;
  return 0;
}

int get_const()
{
  for(int i=0; i<consts_len; i++)
    if(!strcmp(consts[i], word))
      return consts_value[i];

  error_unknown_word();
}

void compile_string(FILE *in)
{
  char string[4096];
  int index=0;

  while(1)
    {
      int c = getc(in);
      switch(c)
        {
        case EOF:
          error_eof();
          break;
        case '"':
          string[index++]=0;
          strings[strings_len++]=malloc(strlen(string));
          strcpy(strings[strings_len-1], string);
          printf("\tdw LIT,string%d,LIT,%d\t; \"%s\"\n", strings_len, strlen(string), strings[strings_len-1]);
          return;
        default:
          string[index++]=c;
          break;
        }
    }
}

void interpret(FILE *in)
{
  if(exists_const())
    psp[psp_index++] = get_const();
  else if(!strcmp(word, "HEX"))
    base = 16;
  else if(!strcmp(word, "DEC"))
    base = 10;
  else if(!strcmp(word, "BIN"))
    base = 2;
  else if(!strcmp(word, "["))
    state = 0;
  else if(!strcmp(word, "]"))
    state = 1;
  else if(!strcmp(word, "+"))
    {
      int i=PSP_POP();
      int j=PSP_POP();
      PSP_PUSH(i+j);
    }
  else if(!strcmp(word, "-"))
    {
      int i=PSP_POP();
      int j=PSP_POP();
      PSP_PUSH(i-j);
    }
  else if(!strcmp(word, ".S"))
    {
      for(int i=0; i<psp_index; i++)
        printf("%d ", psp[i]);
      printf("\n");
    }
  else if(!strcmp(word, "LIT"))
    lit(psp[--psp_index]);
  else if(!strcmp(word, "CREATE"))
    create(in);
  else if(!strcmp(word, "EXIT"))
    eexit();
  else if(!strcmp(word, ":ASM"))
    {
      next_word(in);
      printf("%s:\n", word);
      state = 2;
    }
  else if(!strcmp(word, ":"))
    {
      create(in);
      state = 1;
    }
  else if(!strcmp(word, "("))
    {
      int c;
      while (c!=')')
        {
          c=getc(in);
          if(c==EOF)
            error_eof();
        }
    }
  else if(!strcmp(word, "CONST"))
    {
      next_word(in);
      strcpy(consts[consts_len++], word);
      consts_value[consts_len-1]=psp[--psp_index];
    }
  else if(!strcmp(word, "INCLUDE"))
    {
      next_word(in);
      FILE *fin = fopen(word, "r");
      if(fin == NULL)
        error_file_not_found(word);
      compile(fin);
      fclose(fin);
    }
  else if(is_number())
    psp[psp_index++] = to_number();
  else
    error_unknown_word();
}

void compile_word(FILE *in)
{
  if(exists_const())
    lit(get_const());
  else if(is_immediate())
    interpret(in);
  else if(!strcmp(word, "IF"))
    {
      RSP_PUSH(labels);
      printf("\tdw QBRANCH,label%d\n", labels++);
    }
  else if(!strcmp(word, "ELSE"))
    {
      int label_n = RSP_POP();
      RSP_PUSH(labels);
      printf("\tdw BRANCH,label%d\n", labels++);
      printf("label%d:\n", label_n);
    }
  else if(!strcmp(word, "THEN"))
    {
      int label_n = RSP_POP();
      printf("label%d:\n", label_n);
    }
  else if(!strcmp(word, "DO"))
    {
      printf("\tdw XDO\n");
      RSP_PUSH(labels);
      printf("label%d:\n", labels++);
    }
  else if(!strcmp(word, "LOOP"))
    {
      int label_n = RSP_POP();
      printf("\tdw XLOOP,label%d\n", label_n);
    }
  else if(!strcmp(word, "S\""))
    compile_string(in);
  else if(!strcmp(word, "HERE"))
    printf("\tdw LIT,$-1\n");
  else if(!strcmp(word, "RECURSE"))
    printf("\tdw BRANCH,.begin\n");
  else if(!strcmp(word, ";"))
    {
      eexit();
      state = 0;
    }
  else if(!strcmp(word, "[CHAR]"))
    {
      next_word(in);
      if(strlen(word) > 1)
        error_unknown_word();
      lit(word[0]);
    }
  else if(is_number())
    lit(to_number());
  else
    printf("\tdw %s\n", word);
}

void compile_asm(FILE *in)
{
  // next line
  char line[256];
  if(fgets(line, sizeof(line), in) == NULL)
    error_eof();

  // remove end blank spaces
  int len=strlen(line);
  for(int i=len-1; i>0 && line[i]<=32; i--)
    line[i] = 0;

  // write asm code
  if(strlen(line) >= 4
     && !strcmp(line+strlen(line)-4, "ASM;"))
    {
      if(strlen(line) >= 4)
        {
          line[strlen(line)-4] = 0;
          puts(line);
        }

      printf("\tNEXT\n");
      state = 0;
    }
  else
    puts(line);
}

void compile(FILE *in)
{
  while(1)
    {
      if(state==2)
        compile_asm(in);
      else
        {
          if(!next_word(in))
            break;
          
          switch(state)
            {
            case 0:
              interpret(in);
              break;
              
            case 1:
              compile_word(in);
              break;
            }
        }
    }
}

void puts_file(const char *filename)
{
  FILE *f = fopen(filename, "r");
  while(1)
    {
      char line[256];
      if(fgets(line, sizeof(line), f) == NULL)
        break;
      printf("%s", line);
    }
  fclose(f);
}

void write_kernel()
{
  puts_file("samforth.begin");
}

void write_footer()
{
  for(int i=0; i<strings_len; i++)
    printf("string%d: db \"%s\"\n", i+1, strings[i]);
  
  puts_file("samforth.end");
}

int main(int argc, char **argv)
{
  set_label("?DUP", "QDUP");
  set_label(">R", "TOR");
  set_label("R>", "RFROM");
  set_label("R@", "RFETCH");
  set_label("!", "STORE");
  set_label("C!", "CSTORE");
  set_label("@", "FETCH");
  set_label("C@", "CFETCH");
  set_label("PC!", "PCSTORE");
  set_label("PC@", "PCFECTH");
  set_label("+", "PLUS");
  set_label("-", "MINUS");
  set_label("1+", "ONEPLUS");
  set_label("1-", "ONEMINUS");
  set_label("2*", "TWOSTAR");
  set_label("+!", "PLUSSTORE");
  set_label("?BRANCH", "QBRANCH");
  set_label("(do)", "XDO");
  set_label("(loop)", "XLOOP");
  set_label("(+loop)", "XPLUSLOOP");

  write_kernel();
  if(argc == 0)
    compile(stdin);
  else
    {
      FILE *fin = fopen(argv[1], "r");
      if(fin == NULL)
        error_file_not_found(argv[1]);
      compile(fin);
      fclose(fin);
    }
  write_footer();

  return 0;
}
