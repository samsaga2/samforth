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

def word_comment(input, forth, user_data):
    input.readline()

def word_comment2(input, forth, user_data):
    while 1:
        word = forth.next_word(input)
        if word == ")":
            break
    forth.body = forth.body[0:-1]

def word_dot(input, forth, user_data):
    n = forth.psp.pop()
    print(str(n))

def word_dot_psp(input, forth, user_data):
    print(str(forth.psp))

def word_add(input, forth, user_data):
    n1 = forth.psp.pop()
    n2 = forth.psp.pop()
    forth.psp.append(n2+n1)

def word_sub(input, forth, user_data):
    n1 = forth.psp.pop()
    n2 = forth.psp.pop()
    forth.psp.append(n2-n1)

def word_dup(input, forth, user_data):
    n = forth.psp.pop()
    forth.psp.append(n)
    forth.psp.append(n)

def word_over(input, forth, user_data):
    n1 = forth.psp.pop()
    n2 = forth.psp.pop()
    forth.psp.append(n2)
    forth.psp.append(n1)
    forth.psp.append(n2)

def word_drop(input, forth, user_data):
    forth.psp.pop()

def word_declare(input, forth, user_data):
    forth.state = 1
    forth.body = ""

    # word name
    word = forth.next_word(input)
    if word == "main":
        label = "MAIN"
    else:
        label = forth.new_label()

    # word header
    print("\n\n\t; " + word)
    print(label+":")
    print("\tcall DOCOLON")
    print(".begin:")

    # add compilation func
    forth.words.append([word, 1, word_call, [label, word]])

def word_interpret(input, forth, user_data):
    state = forth.state
    forth.state = 0
    forth.interpret(StringIO.StringIO(user_data))
    forth.state = state

def word_end_declare(input, forth, user_data):
    print("\tdw EXIT")
    forth.state = 0
    forth.body = forth.body[0:-1]
    forth.words.append([forth.words[-1][0], 0, word_interpret, forth.body])

# create word only in compiled code, not interpret
def word_end_cdeclare(input, forth, user_data):
    print("\tdw EXIT")
    forth.state = 0

def word_call(input, forth, user_data):
    print("\tdw " + user_data[0] + "\t; " + user_data[1])

def word_declare_asm(input, forth, user_data):
    forth.body = ""

    word = forth.next_word(input)
    if word == "main":
        label = "MAIN"
    else:
        label = forth.new_label()

    print("\n\n\t; " + word)
    print(label+":")
    forth.words.append([word, 1, word_call, [label, word]])
    
    while 1:
        line = input.readline().strip()
        if line==";asm":
            print("\tNEXT")
            break
        elif line==";asmhl":
            print("\tNEXTHL")
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

def word_tor(input, forth, user_data):
    n = forth.psp.pop()
    forth.rsp.append(n)

def word_rfrom(input, fort, user_data):
    n = forth.rsp.pop()
    forth.psp.append(n)

def word_create(input, forth, user_data):
    word = forth.next_word(input)
    forth.body = ""
    label = forth.new_label()

    print("\n\n\t; " + word)
    print(label+":")
    forth.words.append([word, 1, word_call, [label, word]])

def word_hex(input, forth, user_data):
    forth.base = 16

def word_binary(input, forth, user_data):
    forth.base = 2

def word_decimal(input, forth, user_data):
    forth.base = 10

def word_recurse(input, forth, user_data):
    forth.execute("branch", 1, input)
    print("\tdw .begin\n");
    
def word_if(input, forth, user_data):
    label = forth.new_label()
    forth.rsp.append(label)
    forth.execute("?branch", 1, input)
    print("\tdw " + label)

def word_else(input, forth, user_data):
    label = forth.rsp.pop()

    new_label = forth.new_label()
    forth.rsp.append(new_label)
    
    forth.execute("branch", 1, input)
    print("\tdw " + new_label)
    print(label + ":")

def word_then(input, forth, user_data):
    label = forth.rsp.pop()
    print(label + ":")

def word_do(input, forth, user_data):
    forth.execute("(do)", 1, input)

    label = forth.new_label()
    forth.rsp.append(label)
    print(label + ":")

def word_loop(input, forth, user_data):
    forth.execute("(loop)", 1, input)
    label = forth.rsp.pop()
    print("\tdw " + label)

def word_begin(input, forth, user_data):
    label = forth.new_label()
    forth.rsp.append(label)
    print(label + ":")

def word_until(input, forth, user_data):
    label = forth.rsp.pop()
    forth.execute("0=", 1, input)
    forth.execute("0=", 1, input)
    forth.execute("?branch", 1, input)
    print("\tdw " + label)

def word_again(input, forth, user_data):
    label = forth.rsp.pop()
    forth.execute("branch", 1, input)
    print("\tdw " + label)

def word_repeat(input, forth, user_data):
    label_while = forth.rsp.pop()
    label_begin = forth.rsp.pop()

    forth.execute("branch", 1, input)
    print("\tdw " + label_begin)
    print(label_while + ":")

def word_while(input, forth, user_data):
    label = forth.new_label()
    forth.rsp.append(label)
    forth.execute("?branch", 1, input)
    print("\tdw " + label)

def word_variable(input, forth, user_data):
    word = forth.next_word(input)
    var = ": " + word + " " + hex(forth.freeram)  + " ;"
    forth.interpret(StringIO.StringIO(var))
    forth.freeram += 2

def word_array(input, forth, user_data):
    len = forth.psp.pop()
    word = forth.next_word(input)
    var = ": " + word + " " + hex(forth.freeram)  + " ;"
    forth.interpret(StringIO.StringIO(var))
    forth.freeram += len*2

def word_lit(input, forth, user_data):
    forth.psp.append(user_data)

def word_lit_compile(input, forth, user_data):
    print("\tdw LIT," + str(user_data))

def word_const(input, forth, user_data):
    word = forth.next_word(input)
    n = forth.psp.pop()
    forth.words.append([word, 0, word_lit, n])
    forth.words.append([word, 1, word_lit_compile, n])

def word_include(input, forth, user_data):
    word = forth.next_word(input)
    file = open(word, 'r')
    forth.interpret(file)
    file.close()

def word_cappend(input, forth, user_data):
    n = forth.psp.pop()
    print("\tdb " + str(n))

def word_append(input, forth, user_data):
    n = forth.psp.pop()
    print("\tdw " + str(n))

def word_string(input, forth, user_data):
    s = "";
    while 1:
        c = input.read(1)
        if len(c) == 0 or c[0] == '"':
            break
        elif c == '\\':
            c = input.read(1)
            s += c
        s += c

    label = forth.new_label()
    forth.strings.append([label, s])
    print("\tdw LIT,"+label+",LIT,"+str(len(s)) + "; \"" + s + "\"")

def word_type_string(input, forth, user_data):
    word_string(input, forth, user_data)
    forth.execute("type", 1, input)

def word_char(input, forth, user_data):
    word = forth.next_word(input)
    forth.psp.push(ord(word))

def word_compile_char(input, forth, user_data):
    word = forth.next_word(input)
    print("\tdw LIT," + str(ord(word)))
    
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
        # self.words.append([ name , 0=>intepret/1=>compiled/2=>both , function , user args])
        self.words.append(["\\", 2, word_comment, None])
        self.words.append(["(", 2, word_comment2, None])
        self.words.append([".", 0, word_dot, None])
        self.words.append([".s", 0, word_dot_psp, None])
        self.words.append(["+", 0, word_add, None])
        self.words.append(["-", 0, word_sub, None])
        self.words.append(["dup", 0, word_dup, None])
        self.words.append(["drop", 0, word_drop, None])
        self.words.append(["over", 0, word_over, None])
        self.words.append([":", 0, word_declare, None])
        self.words.append([";", 1, word_end_declare, None])
        self.words.append(["c:", 0, word_declare, None])
        self.words.append([";c", 1, word_end_cdeclare, None])
        self.words.append(["asm:", 0, word_declare_asm, None])
        self.words.append([">r", 0, word_tor, None])
        self.words.append(["r>", 0, word_rfrom, None])
        self.words.append(["create", 0, word_create, None])
        self.words.append(["hex", 0, word_hex, None])
        self.words.append(["binary", 0, word_binary, None])
        self.words.append(["decimal", 0, word_decimal, None])
        self.words.append(["recurse", 1, word_recurse, None])
        self.words.append(["if", 1, word_if, None])
        self.words.append(["then", 1, word_then, None])
        self.words.append(["else", 1, word_else, None])
        self.words.append(["do", 1, word_do, None])
        self.words.append(["loop", 1, word_loop, None])
        self.words.append(["begin", 1, word_begin, None])
        self.words.append(["until", 1, word_until, None])
        self.words.append(["again", 1, word_again, None])
        self.words.append(["repeat", 1, word_repeat, None])
        self.words.append(["while", 1, word_while, None])
        self.words.append(["variable", 0, word_variable, None])
        self.words.append(["array", 0, word_array, None])
        self.words.append(["const", 0, word_const, None])
        self.words.append(["INCLUDE", 0, word_include, None])
        self.words.append(["c,", 0, word_cappend, None])
        self.words.append([",", 0, word_append, None])
        self.words.append(["s\"", 1, word_string, None])
        self.words.append([".\"", 1, word_type_string, None])
        self.words.append(["[char]", 0, word_char, None])
        self.words.append(["[char]", 1, word_compile_char, None])

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
        print("\n")
        for s in self.strings:
            print(s[0] + ":\tdb \"" + s[1] + "\"")

    def execute(self, word, state, input):
        word = self.find_word(word, state)
        word[0](input, self, word[1])

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
                word_func(input, self, user_data)
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
                        print("\tdw LIT,"+str(n))
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
    f.compile()
