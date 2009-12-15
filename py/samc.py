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

        # self.words.append([ name , 0=>intepret/1=>compiled/2=>both , function , user args])
        words = [
            ["\\", 2, self.word_comment, None],
            ["(", 2, self.word_comment2, None],
            [".", 0, self.word_dot, None],
            [".s", 0, self.word_dot_psp, None],
            ["+", 0, self.word_add, None],
            ["-", 0, self.word_sub, None],
            ["dup", 0, self.word_dup, None],
            ["drop", 0, self.word_drop, None],
            ["over", 0, self.word_over, None],
            [":", 0, self.word_declare, None],
            [";", 1, self.word_end_declare, None],
            ["c:", 0, self.word_declare, None],
            [";c", 1, self.word_end_cdeclare, None],
            ["asm:", 0, self.word_declare_asm, None],
            [">r", 0, self.word_tor, None],
            ["r>", 0, self.word_rfrom, None],
            ["create", 0, self.word_create, None],
            ["hex", 0, self.word_hex, None],
            ["binary", 0, self.word_binary, None],
            ["decimal", 0, self.word_decimal, None],
            ["recurse", 1, self.word_recurse, None],
            ["if", 1, self.word_if, None],
            ["then", 1, self.word_then, None],
            ["else", 1, self.word_else, None],
            ["do", 1, self.word_do, None],
            ["loop", 1, self.word_loop, None],
            ["begin", 1, self.word_begin, None],
            ["until", 1, self.word_until, None],
            ["again", 1, self.word_again, None],
            ["repeat", 1, self.word_repeat, None],
            ["while", 1, self.word_while, None],
            ["variable", 0, self.word_variable, None],
            ["array", 0, self.word_array, None],
            ["const", 0, self.word_const, None],
            ["INCLUDE", 0, self.word_include, None],
            ["c,", 0, self.word_cappend, None],
            [",", 0, self.word_append, None],
            ["s\"", 1, self.word_string, None],
            [".\"", 1, self.word_type_string, None],
            ["[char]", 0, self.word_char, None],
            ["[char]", 1, self.word_compile_char, None],
            ["[']", 1, self.word_append_addr, None]
        ]
        for w in words:
            self.forth.words.append(w)

    def word_comment(self, input, user_data):
        input.readline()
    
    def word_comment2(self, input, user_data):
        while 1:
            word = self.forth.next_word(input)
            if word == ")":
                break
        self.forth.body = self.forth.body[0:-1]
    
    def word_dot(self, input, user_data):
        n = self.forth.psp.pop()
        print str(n)
    
    def word_dot_psp(self, input, user_data):
        print str(self.forth.psp)
    
    def word_add(self, input, user_data):
        n1 = self.forth.psp.pop()
        n2 = self.forth.psp.pop()
        self.forth.psp.append(n2+n1)
    
    def word_sub(self, input, user_data):
        n1 = self.forth.psp.pop()
        n2 = self.forth.psp.pop()
        self.forth.psp.append(n2-n1)
    
    def word_dup(self, input, user_data):
        n = self.forth.psp.pop()
        self.forth.psp.append(n)
        self.forth.psp.append(n)
    
    def word_over(self, input, user_data):
        n1 = self.forth.psp.pop()
        n2 = self.forth.psp.pop()
        self.forth.psp.append(n2)
        self.forth.psp.append(n1)
        self.forth.psp.append(n2)
    
    def word_drop(self, input, user_data):
        self.forth.psp.pop()
    
    def word_declare(self, input, user_data):
        self.forth.state = 1
        self.forth.body = ""
    
        # word name
        word = self.forth.next_word(input)
        if word == "main":
            label = "MAIN"
        else:
            label = self.forth.new_label()
    
        # word header
        print "\n\n\t; " + word
        print label+":"
        print "\tcall DOCOLON"
        print ".begin:"
    
        # add compilation func
        self.forth.words.append([word, 1, self.word_call, [label, word]])
    
    def word_interpret(self, input, user_data):
        state = self.forth.state
        self.forth.state = 0
        self.forth.interpret(StringIO.StringIO(user_data))
        self.forth.state = state
    
    def word_end_declare(self, input, user_data):
        print "\tdw EXIT"
        self.forth.state = 0
        self.forth.body = self.forth.body[0:-1]
        self.forth.words.append([self.forth.words[-1][0], 0, self.word_interpret, self.forth.body])
    
    # create word only in compiled code, not interpret
    def word_end_cdeclare(self, input, user_data):
        print "\tdw EXIT"
        self.forth.state = 0
    
    def word_call(self, input, user_data):
        print "\tdw " + user_data[0] + "\t; " + user_data[1]
    
    def word_declare_asm(self, input, user_data):
        self.forth.body = ""
    
        word = self.forth.next_word(input)
        if word == "main":
            label = "MAIN"
        else:
            label = self.forth.new_label()
    
        print "\n\n\t; " + word
        print label+":"
        self.forth.words.append([word, 1, self.word_call, [label, word]])
        
        while 1:
            line = input.readline().strip()
            if line==";asm":
                print "\tNEXT"
                break
            elif line==";asmhl":
                print "\tNEXTHL"
                break
            else:
                has_comment = line.find(";")
                has_label = line.find(":")
                is_label = len(line) > 0 and (line[0] == "." or has_label>0 and has_comment==-1 or has_label>0 and has_label<has_comment)
                has_equ = line.lower().find(" equ ") > 0
                
                if is_label or has_equ:
                   print line
                else:
                    print "\t" + line
    
    def word_tor(self, input, user_data):
        n = self.forth.psp.pop()
        self.forth.rsp.append(n)
    
    def word_rfrom(self, input, user_data):
        n = self.forth.rsp.pop()
        self.forth.psp.append(n)
    
    def word_create(self, input, user_data):
        word = self.forth.next_word(input)
        self.forth.body = ""
        label = self.forth.new_label()
    
        print "\n\n\t; " + word
        print label+":"
        self.forth.words.append([word, 1, self.word_call, [label, word]])
    
    def word_hex(self, input, user_data):
        self.forth.base = 16
    
    def word_binary(self, input, user_data):
        self.forth.base = 2
    
    def word_decimal(self, input, user_data):
        self.forth.base = 10
    
    def word_recurse(self, input, user_data):
        self.forth.execute("branch", 1, input)
        print "\tdw .begin\n"
        
    def word_if(self, input, user_data):
        label = self.forth.new_label()
        self.forth.rsp.append(label)
        self.forth.execute("?branch", 1, input)
        print "\tdw " + label
    
    def word_else(self, input, user_data):
        label = self.forth.rsp.pop()
    
        new_label = self.forth.new_label()
        self.forth.rsp.append(new_label)
        
        self.forth.execute("branch", 1, input)
        print "\tdw " + new_label
        print label + ":"
    
    def word_then(self, input, user_data):
        label = self.forth.rsp.pop()
        print label + ":"
    
    def word_do(self, input, user_data):
        self.forth.execute("(do)", 1, input)
    
        label = self.forth.new_label()
        self.forth.rsp.append(label)
        print label + ":"
    
    def word_loop(self, input, user_data):
        self.forth.execute("(loop)", 1, input)
        label = self.forth.rsp.pop()
        print "\tdw " + label
    
    def word_begin(self, input, user_data):
        label = self.forth.new_label()
        self.forth.rsp.append(label)
        print label + ":"
    
    def word_until(self, input, user_data):
        label = self.forth.rsp.pop()
        self.forth.execute("0=", 1, input)
        self.forth.execute("0=", 1, input)
        self.forth.execute("?branch", 1, input)
        print "\tdw " + label
    
    def word_again(self, input, user_data):
        label = self.forth.rsp.pop()
        self.forth.execute("branch", 1, input)
        print "\tdw " + label
    
    def word_repeat(self, input, user_data):
        label_while = self.forth.rsp.pop()
        label_begin = self.forth.rsp.pop()
    
        self.forth.execute("branch", 1, input)
        print "\tdw " + label_begin
        print label_while + ":"
    
    def word_while(self, input, user_data):
        label = self.forth.new_label()
        self.forth.rsp.append(label)
        self.forth.execute("?branch", 1, input)
        print "\tdw " + label
    
    def word_variable(self, input, user_data):
        word = self.forth.next_word(input)
        var = ": " + word + " " + hex(self.forth.freeram)  + " ;"
        self.forth.interpret(StringIO.StringIO(var))
        self.forth.freeram += 2
    
    def word_array(self, input, user_data):
        len = self.forth.psp.pop()
        word = self.forth.next_word(input)
        var = ": " + word + " " + hex(self.forth.freeram)  + " ;"
        self.forth.interpret(StringIO.StringIO(var))
        self.forth.freeram += len*2
    
    def word_lit(self, input, user_data):
        self.forth.psp.append(user_data)
    
    def word_lit_compile(self, input, user_data):
        print "\tdw LIT," + str(user_data)
    
    def word_const(self, input, user_data):
        word = self.forth.next_word(input)
        n = self.forth.psp.pop()
        self.forth.words.append([word, 0, self.word_lit, n])
        self.forth.words.append([word, 1, self.word_lit_compile, n])
    
    def word_include(self, input, user_data):
        word = self.forth.next_word(input)
        file = open(word, 'r')
        self.forth.interpret(file)
        file.close()
    
    def word_cappend(self, input, user_data):
        n = self.forth.psp.pop()
        print "\tdb " + str(n)
    
    def word_append(self, input, user_data):
        n = self.forth.psp.pop()
        print "\tdw " + str(n)
    
    def word_string(self, input, user_data):
        s = "";
        while 1:
            c = input.read(1)
            if len(c) == 0 or c[0] == '"':
                break
            elif c == '\\':
                c = input.read(1)
                s += c
            s += c
    
        label = self.forth.new_label()
        self.forth.strings.append([label, s])
        print "\tdw LIT,"+label+",LIT,"+str(len(s)) + "; \"" + s + "\""
    
    def word_type_string(self, input, user_data):
        self.word_string(input, user_data)
        self.forth.execute("type", 1, input)
    
    def word_char(self, input, user_data):
        word = self.forth.next_word(input)
        self.forth.psp.push(ord(word))
    
    def word_compile_char(self, input, user_data):
        word = self.forth.next_word(input)
        print "\tdw LIT," + str(ord(word))
    
    def word_append_addr(self, input, user_data):
        word = self.forth.next_word(input)
        print "\tdw LIT"
        self.forth.execute(word, 1, input)
        
class Forth:
    def __init__(self):
        self.state = 0 # 0 => interpret / 1 => compiling
        self.psp = []
        self.rsp = []
        self.labels = 0
        self.body = ""
        self.base = 10
        self.freeram = 0xe500
        self.strings = []
        self.words = []

    def new_label(self):
        self.labels += 1
        return "label"+str(self.labels);

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
                return [w[2], w[3]]
        return [None, None]

    def add_strings(self):
        print "\n"
        for s in self.strings:
            print s[0] + ":\tdb \"" + s[1] + "\""

    def execute(self, word, state, input):
        word = self.find_word(word, state)
        word[0](input, word[1])

    def add_header(self):
        print_file("samforth.begin")

    def add_footer(self):
        print_file("samforth.end")

    def interpret(self, input=sys.stdin):
        state = 0
        while 1:
            # get next word
            word = self.next_word(input)
            if word == None:
                break
            self.body += " " + word

            # find word
            [word_func, user_data] = self.find_word(word, self.state)
            if word_func == None:
                [word_func, user_data] = self.find_word(word, 2)

            if word_func != None:
                # execute it
                word_func(input, user_data)
            else:
                try:
                    # try literal
                    if word[0:2] == "0x" or word[0:2] == "0b":
                        n = int(word, 0)
                    else:
                        n = int(word, self.base)

                    if self.state == 0:
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

