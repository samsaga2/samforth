#!/usr/bin/python

import sys
import string
import StringIO

class UnableCompile(Exception):
    def __init__(self, word):
        self.word = word

    def __str__(self):
        return "Unable to compile `" + self.word + "'"

def print_file(filename):
    file = open(filename, "r")
    while True:
        line = file.readline()
        if len(line) == 0:
            break
        print line,
    file.close()

class CoreWords:
    def __init__(self, forth):
        self.forth = forth

        # [ name , 0=>intepret/1=>compiled/2=>both , function ]
        words = [
            ["\\", 2, self.w_comment],
            ["(", 2, self.w_comment2],
            [".", 0, self.w_dot],
            [".s", 0, self.w_dot_psp],
            ["+", 0, self.w_add],
            ["-", 0, self.w_sub],
            ["*", 0, self.w_mul],
            ["/", 0, self.w_div],
            ["dup", 0, self.w_dup],
            ["drop", 0, self.w_drop],
            ["swap", 0, self.w_swap],
            ["over", 0, self.w_over],
            [":", 0, self.w_declare],
            [";", 1, self.w_end_declare],
            ["i:", 0, self.w_ideclare],
            ["c:", 0, self.w_declare],
            [";c", 1, self.w_end_cdeclare],
            ["asm:", 0, self.w_declare_asm],
            [">r", 0, self.w_tor],
            ["r>", 0, self.w_rfrom],
            ["create", 0, self.w_create],
            ["hex", 0, self.w_hex],
            ["binary", 0, self.w_binary],
            ["decimal", 0, self.w_decimal],
            ["recurse", 1, self.w_recurse],
            ["if", 1, self.w_if],
            ["then", 1, self.w_then],
            ["else", 1, self.w_else],
            ["do", 1, self.w_do],
            ["loop", 1, self.w_loop],
            ["begin", 1, self.w_begin],
            ["until", 1, self.w_until],
            ["again", 1, self.w_again],
            ["repeat", 1, self.w_repeat],
            ["while", 1, self.w_while],
            ["variable", 0, self.w_variable],
            ["array", 0, self.w_array],
            ["const", 0, self.w_const],
            ["INCLUDE", 0, self.w_include],
            ["c,", 0, self.w_cappend],
            [",", 0, self.w_append],
            ["s\"", 1, self.w_string],
            [".\"", 1, self.w_type_string],
            ["[char]", 0, self.w_char],
            ["[char]", 1, self.w_compile_char],
            ["[']", 1, self.w_append_addr],
            ["parse-name", 0, self.w_parse_name],
            ["evaluate", 0, self.w_evaluate]
        ]
        for w in words:
            self.forth.words.append(w)

    def w_comment(self, input, input_code):
        input.readline()
    
    def w_comment2(self, input, input_code):
        while 1:
            word = self.forth.next_word(input)
            if word == ")":
                break
    
    def w_dot(self, input, input_code):
        n = self.forth.psp.pop()
        sys.stderr.write(str(n) + "\n")
    
    def w_dot_psp(self, input, input_code):
        sys.stderr.write(str(self.forth.psp) + "\n")
    
    def w_add(self, input, input_code):
        n1 = self.forth.psp.pop()
        n2 = self.forth.psp.pop()
        self.forth.psp.append(n2+n1)
    
    def w_sub(self, input, input_code):
        n1 = self.forth.psp.pop()
        n2 = self.forth.psp.pop()
        self.forth.psp.append(n2-n1)
    
    def w_mul(self, input, input_code):
        n1 = self.forth.psp.pop()
        n2 = self.forth.psp.pop()
        self.forth.psp.append(n2*n1)
    
    def w_div(self, input, input_code):
        n1 = self.forth.psp.pop()
        n2 = self.forth.psp.pop()
        self.forth.psp.append(n2/n1)
    
    def w_dup(self, input, input_code):
        n = self.forth.psp.pop()
        self.forth.psp.append(n)
        self.forth.psp.append(n)
    
    def w_over(self, input, input_code):
        n1 = self.forth.psp.pop()
        n2 = self.forth.psp.pop()
        self.forth.psp.append(n2)
        self.forth.psp.append(n1)
        self.forth.psp.append(n2)
    
    def w_drop(self, input, input_code):
        self.forth.psp.pop()
    
    def w_swap(self, input, input_code):
        n1 = self.forth.psp.pop()
        n2 = self.forth.psp.pop()
        self.forth.psp.append(n1)
        self.forth.psp.append(n2)
    
    def w_declare(self, input, input_code):
        self.forth.state = 1
    
        # word name
        word = self.forth.next_word(input)
        if word == "main":
            label = "MAIN"
        else:
            label = self.forth.new_label()
    
        # word header
        self.forth.emit_asm("\n\n\t; " + word)
        self.forth.emit_asm(label+":")
        self.forth.emit_asm("call DOCOLON")
        self.forth.emit_asm(".begin:")
    
        # add compilation func
        func = lambda input,input_code: self.w_call(input, input_code, label, word)
        self.forth.words.append([word, 1, func])

    def w_ideclare(self, input, input_code):
        name = self.forth.next_word(input)
        body = ""
        while 1:
            word = self.forth.next_word(input)
            if word == ";i":
                break
            elif word == "(":
                while word != ")":
                    word = self.forth.next_word(input)
                continue
            elif word == "\\":
                input.readline()
                continue
            else:
                body += " " + word

        # add interpreted word
        func = lambda input,input_code: self.w_interpret(input, input_code, body)
        self.forth.words.append([name, 0, func])
    
    def w_interpret(self, input, input_code, code):
        state = self.forth.state
        self.forth.state = 2
        self.forth.interpret(input, StringIO.StringIO(code))
        self.forth.state = state
    
    def w_end_declare(self, input, input_code):
        self.forth.emit_asm("dw EXIT")

        # change to interpret state
        self.forth.state = 0
    
    # create word only in compiled code, not interpret
    def w_end_cdeclare(self, input, input_code):
        self.forth.emit_asm("dw EXIT")
        self.forth.state = 0
    
    def w_call(self, input, input_code, label, word):
        self.forth.emit_asm("dw " + label + "\t; " + word)
    
    def w_declare_asm(self, input, input_code):
        word = self.forth.next_word(input)
        if word == "main":
            label = "MAIN"
        else:
            label = self.forth.new_label()

        self.forth.emit_asm("\n\n\t; " + word)
        self.forth.emit_asm(label+":")

        func = lambda input,input_code: self.w_call(input, input_code, label, word)
        self.forth.words.append([word, 1, func])
        
        while 1:
            line = input.readline().strip()
            if line==";asm":
                self.forth.emit_asm("NEXT")
                break
            elif line==";asmhl":
                self.forth.emit_asm("NEXTHL")
                break
            else:
                self.forth.emit_asm(line)
    
    def w_tor(self, input, input_code):
        n = self.forth.psp.pop()
        self.forth.rsp.append(n)
    
    def w_rfrom(self, input, input_code):
        n = self.forth.rsp.pop()
        self.forth.psp.append(n)
    
    def w_create(self, input, input_code):
        word = self.forth.next_word(input)
        label = self.forth.new_label()

        self.forth.emit_asm("\n\n\t; " + word)
        self.forth.emit_asm(label+":")

        func = lambda input,input_code: self.w_call(input, input_code, label, word)
        self.forth.words.append([word, 1, func])
    
    def w_hex(self, input, input_code):
        self.forth.base = 16
    
    def w_binary(self, input, input_code):
        self.forth.base = 2
    
    def w_decimal(self, input, input_code):
        self.forth.base = 10
    
    def w_recurse(self, input, input_code):
        self.forth.execute("branch", 1, input)
        self.forth.emit_asm("dw .begin")
        
    def w_if(self, input, input_code):
        label = self.forth.new_label()
        self.forth.rsp.append(label)
        self.forth.execute("?branch", 1, input)
        self.forth.emit_asm("dw " + label)
    
    def w_else(self, input, input_code):
        label = self.forth.rsp.pop()
    
        new_label = self.forth.new_label()
        self.forth.rsp.append(new_label)
        
        self.forth.execute("branch", 1, input)
        self.forth.emit_asm("dw " + new_label)
        self.forth.emit_asm(label + ":")
    
    def w_then(self, input, input_code):
        label = self.forth.rsp.pop()
        self.forth.emit_asm(label + ":")
    
    def w_do(self, input, input_code):
        self.forth.execute("(do)", 1, input)
    
        label = self.forth.new_label()
        self.forth.rsp.append(label)
        self.forth.emit_asm(label + ":")
    
    def w_loop(self, input, input_code):
        self.forth.execute("(loop)", 1, input)
        label = self.forth.rsp.pop()
        self.forth.emit_asm("dw " + label)
    
    def w_begin(self, input, input_code):
        label = self.forth.new_label()
        self.forth.rsp.append(label)
        self.forth.emit_asm(label + ":")
    
    def w_until(self, input, input_code):
        label = self.forth.rsp.pop()
        self.forth.execute("0=", 1, input)
        self.forth.execute("0=", 1, input)
        self.forth.execute("?branch", 1, input)
        self.forth.emit_asm("dw " + label)
    
    def w_again(self, input, input_code):
        label = self.forth.rsp.pop()
        self.forth.execute("branch", 1, input)
        self.forth.emit_asm("dw " + label)
    
    def w_repeat(self, input, input_code):
        label_while = self.forth.rsp.pop()
        label_begin = self.forth.rsp.pop()
    
        self.forth.execute("branch", 1, input)
        self.forth.emit_asm("dw " + label_begin)
        self.forth.emit_asm(label_while + ":")
    
    def w_while(self, input, input_code):
        label = self.forth.new_label()
        self.forth.rsp.append(label)
        self.forth.execute("?branch", 1, input)
        self.forth.emit_asm("\tdw " + label)
    
    def w_variable(self, input, input_code):
        word = self.forth.next_word(input_code)
        var = ": " + word + " " + hex(self.forth.freeram)  + " ;"
        self.forth.interpret(StringIO.StringIO(var))
        self.forth.freeram += 2
    
    def w_array(self, input, input_code):
        len = self.forth.psp.pop()
        word = self.forth.next_word(input_code)
        var = ": " + word + " " + hex(self.forth.freeram)  + " ;"
        self.forth.interpret(StringIO.StringIO(var))
        self.forth.freeram += len*2
    
    def w_lit(self, input, input_code, value):
        self.forth.psp.append(value)
    
    def w_lit_compile(self, input, input_code, value):
        self.forth.emit_asm("dw LIT," + str(value))
    
    def w_const(self, input, input_code):
        word = self.forth.next_word(input_code)
        n = self.forth.psp.pop()

        func = lambda input,input_code: self.w_lit(input, input_code, n)
        self.forth.words.append([word, 0, func])

        func = lambda input,input_code: self.w_lit_compile(input, input_code, n)
        self.forth.words.append([word, 1, func])
    
    def w_include(self, input, input_code):
        word = self.forth.next_word(input_code)
        file = open(word, 'r')
        self.forth.interpret(file)
        file.close()
    
    def w_cappend(self, input, input_code):
        n = self.forth.psp.pop()
        self.forth.emit_asm("db " + str(n))
    
    def w_append(self, input, input_code):
        n = self.forth.psp.pop()
        self.forth.emit_asm("dw " + str(n))
    
    def w_string(self, input, input_code):
        s = "";
        while 1:
            c = input_code.read(1)
            if len(c) == 0 or c[0] == '"':
                break
            elif c == '\\':
                c = input_code.read(1)
                s += c
            s += c
    
        label = self.forth.new_label()
        self.forth.strings.append([label, s])
        self.forth.emit_asm("dw LIT,"+label+",LIT,"+str(len(s)) + "; \"" + s + "\"")
    
    def w_type_string(self, input, input_code):
        self.w_string(input, input_code)
        self.forth.execute("type", 1, input)
    
    def w_char(self, input, input_code):
        word = self.forth.next_word(input)
        self.forth.psp.append(ord(word))
    
    def w_compile_char(self, input, input_code):
        word = self.forth.next_word(input)
        self.forth.emit_asm("dw LIT," + str(ord(word)))
    
    def w_append_addr(self, input, input_code):
        word = self.forth.next_word(input)
        self.forth.emit_asm("dw LIT")
        self.forth.execute(word, 1, input)

    def w_parse_name(self, input, input_code):
        word = self.forth.next_word(input)
        self.forth.psp.append(word)

    def w_evaluate(self, input, input_code):
        code = self.forth.psp.pop()
        self.w_interpret(input, input_code, code)
        
class Forth:
    def __init__(self):
        self.state = 0 # 0 => interpret / 1 => compiling / 2 => executing word
        self.psp = [] # parameter stack
        self.rsp = [] # return stack
        self.labels = 0
        self.base = 10
        self.freeram = 0xe500 # start of free ram
        self.strings = []
        self.words = [] # words dictionary

    def new_label(self):
        self.labels += 1
        return "label"+str(self.labels);

    def emit_asm(self, code):
        has_comment = code.find(";")
        has_label = code.find(":")
        is_label = len(code) > 0 and (code[0] == "." or has_label>0 and has_comment==-1 or has_label>0 and has_label<has_comment)
        has_equ = code.lower().find(" equ ") > 0
        
        if is_label or has_equ:
            print code
        else:
            print "\t" + code

    def next_word(self, input):
        # skip whitespaces
        while 1:
            c = input.read(1)
            if len(c) == 0:
                return None
            if c[0] != ' ' and c[0] != '\t' and ord(c[0]) != 13 and ord(c[0]) != 10:
                break

        # read word
        word = c;
        while 1:
            c = input.read(1)
            if len(c) == 0 or c[0] == ' ' or c[0] == '\t' or ord(c[0]) == 13 or ord(c[0]) == 10:
                break
            word += c

        return word

    def find_word(self, word, word_type):
        for w in reversed(self.words):
            if w[0] == word and w[1] == word_type:
                return w[2]
        return None

    def add_strings(self):
        print "\n"
        for s in self.strings:
            print s[0] + ":\tdb \"" + s[1] + "\""

    def execute(self, word, state, input, input_code=None):
        if input_code == None:
            input_code = input

        word = self.find_word(word, state)
        word(input, input_code)

    def add_header(self):
        print_file("samforth.begin")

    def add_footer(self):
        print_file("samforth.end")

    def interpret(self, input=sys.stdin, input_code=None):
        if input_code == None:
            input_code = input

        while 1:
            # get next word
            word = self.next_word(input_code)
            if word == None:
                break

            if self.state == 2:
                word_func = self.find_word(word, 0)
                if word_func == None:
                    word_func = self.find_word(word, 2)
            else:
                # find word
                word_func = self.find_word(word, self.state)
                if word_func == None:
                    word_func = self.find_word(word, 2)

            if word_func != None:
                # execute it
                word_func(input, input_code)
            else:
                try:
                    # try literal
                    if word[0:2] == "0x" or word[0:2] == "0b":
                        n = int(word, 0)
                    else:
                        n = int(word, self.base)

                    if self.state == 0 or self.state == 2:
                        self.psp.append(n)
                    elif self.state == 1:
                        print "\tdw LIT,"+str(n)
                except:
                    sys.stderr.write("Unknown word `"+word+"/" + str(self.state) + "'")
                    exit(1)

    def compile(self):
        self.add_header()
        self.interpret()
        self.add_strings()
        self.add_footer()

if __name__ == '__main__':
    f = Forth()
    core = CoreWords(f)
    f.compile()

