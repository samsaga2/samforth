#include <stdio.h>
#include <stdlib.h>
#include <limits.h>
#include <errno.h>
#include <string.h>
#include <strings.h>

#define MAX_WORD_LEN 200
#define STACK_SIZE 1000
#define MAX_STRINGS 4000
#define MAX_CONSTS 4000
#define MAX_WORDS 4000
#define MAX_VARS 4000

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

int freeram=0xe500;
char vars[MAX_VARS][MAX_WORD_LEN];
int vars_value[MAX_VARS];
int vars_len=0;

void compile(FILE *in);

int is_space(int c)
{
  return c == ' ' || c == '\t' || c == 13 || c == 10;
}

void translate()
{
  for(int i=0; i<words_len; i++)
    if(!strcasecmp(words[i], word)) {
      if(strcasecmp(word, words_label[i]))
        printf("\t\t;%s=%s\n", word, words_label[i]);
      strcpy(word, words_label[i]);
      break;
    }
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
  if(!strcasecmp(word, "(")
     || !strcasecmp(word, "["))
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
  if(strcasecmp(word, "MAIN"))
    {
      sprintf(label, "label%d", labels++);
      set_label(word, label);
    }
  else
    strcpy(label, "MAIN");

  printf("\n\t; %s\n%s:\n", word, label);
}

void eexit()
{
  printf("\tdw EXIT\n");
}

void lit(int value)
{
  printf("\tdw LIT, %d\n", value);
}

int exists_var()
{
  for(int i=0; i<vars_len; i++)
    if(!strcasecmp(vars[i], word))
      return 1;
  return 0;
}

int get_var()
{
  for(int i=0; i<vars_len; i++)
    if(!strcasecmp(vars[i], word))
      return vars_value[i];

  error_unknown_word();
}

int exists_const()
{
  for(int i=0; i<consts_len; i++)
    if(!strcasecmp(consts[i], word))
      return 1;
  return 0;
}

int get_const()
{
  for(int i=0; i<consts_len; i++)
    if(!strcasecmp(consts[i], word))
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
  if(exists_var())
    psp[psp_index++] = get_var();
  else if(exists_const())
    psp[psp_index++] = get_const();
  else if(!strcasecmp(word, "HEX"))
    base = 16;
  else if(!strcasecmp(word, "DECIMAL"))
    base = 10;
  else if(!strcasecmp(word, "BINARY"))
    base = 2;
  else if(!strcasecmp(word, "OCTAL"))
    base = 8;
  else if(!strcasecmp(word, "["))
    state = 0;
  else if(!strcasecmp(word, "]"))
    state = 1;
  else if(!strcasecmp(word, "+"))
    {
      int i=PSP_POP();
      int j=PSP_POP();
      PSP_PUSH(i+j);
    }
  else if(!strcasecmp(word, "-"))
    {
      int i=PSP_POP();
      int j=PSP_POP();
      PSP_PUSH(i-j);
    }
  else if(!strcasecmp(word, ".S"))
    {
      for(int i=0; i<psp_index; i++)
        printf("%d ", psp[i]);
      printf("\n");
    }
  else if(!strcasecmp(word, "LIT"))
    lit(psp[--psp_index]);
  else if(!strcasecmp(word, "CREATE"))
    {
      create(in);
      printf("\tcall DOCOLON\n");
      printf("\tdw LIT,.begin\n");
      printf("\tdw EXIT\n");
      printf(".begin:\n");
    }
  else if(!strcasecmp(word, ","))
    printf("\tdw %d\n", psp[--psp_index]);
  else if(!strcasecmp(word, "C,"))
    printf("\tdb %d\n", psp[--psp_index]);    
  else if(!strcasecmp(word, "EXIT"))
    eexit();
  else if(!strcasecmp(word, "ASM:"))
    {
      next_word(in);
      char name[256];
      strcpy(name, word);
      next_word(in);
      set_label(name, word);

      printf("\t;; %s\n", name);
      printf("%s:\n", word);
      state = 2;
    }
  else if(!strcasecmp(word, ":"))
    {
      create(in);
      printf("\tcall DOCOLON\n.begin:\n");
      state = 1;
    }
  else if(!strcasecmp(word, "("))
    {
      int c;
      while (c!=')')
        {
          c=getc(in);
          if(c==EOF)
            error_eof();
        }
    }
  else if(!strcasecmp(word, "CONST"))
    {
      next_word(in);
      strcpy(consts[consts_len++], word);
      consts_value[consts_len-1]=psp[--psp_index];
    }
  else if(!strcasecmp(word, "INCLUDE"))
    {
      next_word(in);
      FILE *fin = fopen(word, "r");
      if(fin == NULL)
        error_file_not_found(word);
      compile(fin);
      fclose(fin);
    }
  else if(!strcasecmp(word, "VARIABLE"))
    {
      next_word(in);
      strcpy(vars[vars_len++], word);
      vars_value[vars_len-1]=freeram;
      freeram+=2;
    }
  else if(!strcasecmp(word, "ARRAY"))
    {
      int len = psp[--psp_index];
      next_word(in);
      strcpy(vars[vars_len++], word);
      vars_value[vars_len-1]=freeram;
      freeram+=len*2;
    }
  else if(is_number())
    psp[psp_index++] = to_number();
  else
    error_unknown_word();
}

void compile_word(FILE *in)
{
  translate();

  if(exists_var())
    lit(get_var());
  else if(exists_const())
    lit(get_const());
  else if(is_immediate())
    interpret(in);
  else if(!strcasecmp(word, "IF"))
    {
      RSP_PUSH(labels);
      printf("\tdw qbranch,label%d\n", labels++);
    }
  else if(!strcasecmp(word, "ELSE"))
    {
      int label_n = RSP_POP();
      RSP_PUSH(labels);
      printf("\tdw branch,label%d\n", labels++);
      printf("label%d:\n", label_n);
    }
  else if(!strcasecmp(word, "THEN"))
    {
      int label_n = RSP_POP();
      printf("label%d:\n", label_n);
    }
  else if(!strcasecmp(word, "DO"))
    {
      printf("\tdw xdo\n");
      RSP_PUSH(labels);
      printf("label%d:\n", labels++);
    }
  else if(!strcasecmp(word, "LOOP"))
    {
      int label_n = RSP_POP();
      printf("\tdw xloop,label%d\n", label_n);
    }
  else if(!strcasecmp(word, "S\""))
    compile_string(in);
  else if(!strcasecmp(word, ".\""))
    {
      compile_string(in);
      strcpy(word, "TYPE");
      translate();
      printf("\tdw %s\n", word);
    }
  else if(!strcasecmp(word, "HERE"))
    printf("\tdw LIT,$-1\n");
  else if(!strcasecmp(word, "RECURSE"))
    printf("\tdw branch,.begin\n");
  else if(!strcasecmp(word, ";"))
    {
      eexit();
      state = 0;
    }
  else if(!strcasecmp(word, "[CHAR]"))
    {
      next_word(in);
      if(strlen(word) > 1)
        error_unknown_word();
      lit(word[0]);
    }
  else if(!strcasecmp(word, "BEGIN"))
    {
      RSP_PUSH(labels);
      printf("label%d:\n", labels++);
    }
  else if(!strcasecmp(word, "UNTIL"))
    {
      int label_n = RSP_POP();
      printf("\tdw zeroequal,zeroequal,qbranch,label%d\n", label_n);
    }
  else if(!strcasecmp(word, "AGAIN"))
    {
      printf("\tdw branch,label%s\n", rsp[--rsp_index]);
    }
  else if(!strcasecmp(word, "REPEAT"))
    {
      int label_while = RSP_POP();
      int label_begin = RSP_POP();
      
      // again
      printf("\tdw branch,label%d\n", label_begin);
      
      // then
      printf("label%d:\n", label_while);
    }
  else if(!strcasecmp(word, "WHILE"))
    {
      RSP_PUSH(labels);
      printf("\tdw qbranch,label%d\n", labels++);
    }
  else if(is_number())
    lit(to_number());
  else
    printf("\tdw %s\n", word);
}

void compile_asm(FILE *in)
{
  int c = fgetc(in);
  switch(c)
    {
    case '(':
      c=getc(in);
      if(c!=' ' && c!='\t')
        {
          putchar('(');
          putchar(c);
        }
      else
        while (c!=')')
          {
            c=getc(in);
            if(c==EOF)
              error_eof();
          }
      break;
      
    case ';':
      next_word(in);
      if(!strcasecmp(word, "ASM"))
        {
          printf("\tNEXT\n");
          state = 0;
        }
      else if(!strcasecmp(word, "ASMHL"))
        {
          printf("\tNEXTHL\n");
          state = 0;
        }
      else
        putchar(c);
      break;

    case '\\':
      while (c!=13 && c!=10)
        {
          c=getc(in);
          if(c==EOF)
            error_eof();
        }
      printf("\n");
      break;

    default:
      putchar(c);
      break;
    }
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
