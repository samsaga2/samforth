INCLUDE kernel.fs
INCLUDE bios.fs

create A-pattern
0b11111100 c,
0b10000110 c,
0b10000110 c,
0b10000110 c,
0b11111110 c,
0b10000110 c,
0b10000110 c,
0b00000000 c,
create B-pattern
0b11111100 c,
0b10000110 c,
0b10000110 c,
0b10000110 c,
0b11111110 c,
0b10000110 c,
0b11111110 c,
0b00000000 c,
create C-pattern
0b11111100 c,
0b10000110 c,
0b10000000 c,
0b10000000 c,
0b10000000 c,
0b10000110 c,
0b01111110 c,
0b00000000 c,
create D-pattern
0b11111100 c,
0b10000110 c,
0b10000110 c,
0b10000110 c,
0b10000110 c,
0b10000110 c,
0b11111110 c,
0b00000000 c,
create E-pattern
0b11111100 c,
0b10000000 c,
0b10000000 c,
0b10000000 c,
0b11111110 c,
0b10000000 c,
0b11111110 c,
0b00000000 c,
create F-pattern
0b11111100 c,
0b10000000 c,
0b10000000 c,
0b10000000 c,
0b11111110 c,
0b10000000 c,
0b10000000 c,
0b00000000 c,
create G-pattern
0b11111100 c,
0b10000110 c,
0b10000000 c,
0b10000000 c,
0b11111100 c,
0b10000110 c,
0b01111110 c,
0b00000000 c,
create H-pattern
0b10000100 c,
0b10000110 c,
0b10000110 c,
0b10000110 c,
0b11111110 c,
0b10000110 c,
0b10000110 c,
0b00000000 c,
create I-pattern
0b00010000 c,
0b00011000 c,
0b00011000 c,
0b00011000 c,
0b00011000 c,
0b00011000 c,
0b00011000 c,
0b00000000 c,
create J-pattern
0b11111000 c,
0b00001100 c,
0b00001100 c,
0b00001100 c,
0b00001100 c,
0b10001100 c,
0b01111100 c,
0b00000000 c,
create K-pattern
0b10001000 c,
0b10001100 c,
0b10001100 c,
0b10011000 c,
0b11100000 c,
0b10011100 c,
0b10001100 c,
0b00000000 c,
create L-pattern
0b11000000 c,
0b11000000 c,
0b11000000 c,
0b11000000 c,
0b11000000 c,
0b11000000 c,
0b11111110 c,
0b00000000 c,
create M-pattern
0b11001100 c,
0b10110110 c,
0b10000110 c,
0b10000110 c,
0b10000110 c,
0b10000110 c,
0b10000110 c,
0b00000000 c,
create N-pattern
0b11000110 c,
0b11000110 c,
0b10100110 c,
0b10010110 c,
0b10001110 c,
0b10000110 c,
0b10000110 c,
0b00000000 c,
create O-pattern
0b11111100 c,
0b10000110 c,
0b10000110 c,
0b10000110 c,
0b10000110 c,
0b10000110 c,
0b01111110 c,
0b00000000 c,
create P-pattern
0b11111100 c,
0b10000110 c,
0b10000110 c,
0b10000110 c,
0b11111110 c,
0b10000000 c,
0b10000000 c,
0b00000000 c,
create Q-pattern
0b11111100 c,
0b10000110 c,
0b10000110 c,
0b10000110 c,
0b10010110 c,
0b10001110 c,
0b01111110 c,
0b00000000 c,
create R-pattern
0b11111100 c,
0b10000110 c,
0b10000110 c,
0b10000110 c,
0b11111110 c,
0b10011100 c,
0b10001100 c,
0b00000000 c,
create S-pattern
0b11111000 c,
0b10000000 c,
0b10000000 c,
0b10000000 c,
0b11111100 c,
0b00000110 c,
0b11111110 c,
0b00000000 c,
create T-pattern
0b11111110 c,
0b00011000 c,
0b00011000 c,
0b00011000 c,
0b00011000 c,
0b00011000 c,
0b00011000 c,
0b00000000 c,
create U-pattern
0b10000100 c,
0b10000110 c,
0b10000110 c,
0b10000110 c,
0b10000110 c,
0b10000110 c,
0b11111110 c,
0b00000000 c,
create V-pattern
0b10000100 c,
0b10000110 c,
0b10000110 c,
0b10000110 c,
0b10000110 c,
0b01001110 c,
0b00111100 c,
0b00000000 c,
create W-pattern
0b10000100 c,
0b10000110 c,
0b10000110 c,
0b10000110 c,
0b10110110 c,
0b10110110 c,
0b11111110 c,
0b00000000 c,
create X-pattern
0b10000100 c,
0b11000110 c,
0b00101100 c,
0b00010000 c,
0b00101100 c,
0b11000110 c,
0b10000110 c,
0b00000000 c,
create Y-pattern
0b10000100 c,
0b10000110 c,
0b10000110 c,
0b10000110 c,
0b01111100 c,
0b00011000 c,
0b00011000 c,
0b00000000 c,
create Z-pattern
0b11111100 c,
0b00001110 c,
0b00001100 c,
0b00011000 c,
0b01110000 c,
0b10000000 c,
0b11111110 c,
0b00000000 c,

create char-color
0x90 c,
0x60 c,
0x60 c,
0x60 c,
0x90 c,
0x80 c,
0x80 c,
0x80 c,

: char-vaddr ( c -- v-addr )
    3 lshift ;

: char-color-vaddr ( c -- v-addr )
    3 lshift 0x2000 + ;

: ram-to-3banks ( addr vaddr -- )
    2dup swap 8 -rot ram-to-vram
    2dup 2048 + swap 8 -rot ram-to-vram
    4096 + swap 8 -rot ram-to-vram ;

: redefine-pattern ( addr char -- )
    char-vaddr ram-to-3banks ;

: redefine-color ( addr char -- )
    char-color-vaddr ram-to-3banks ;

: redefine-tile ( pattern color char -- )
    2dup redefine-color
    nip redefine-pattern ;

: redefine-char ( pattern char -- )
    char-color swap redefine-tile ;

: clear-tiles
    0 0x2000 0x2000 fill-vram ;

: redefine-tiles ( -- )
    clear-tiles
    ['] A-pattern [char] A redefine-char
    ['] B-pattern [char] B redefine-char
    ['] C-pattern [char] C redefine-char 
    ['] D-pattern [char] D redefine-char
    ['] E-pattern [char] E redefine-char
    ['] F-pattern [char] F redefine-char
    ['] G-pattern [char] G redefine-char 
    ['] H-pattern [char] H redefine-char
    ['] I-pattern [char] I redefine-char
    ['] J-pattern [char] J redefine-char
    ['] K-pattern [char] K redefine-char 
    ['] L-pattern [char] L redefine-char
    ['] M-pattern [char] M redefine-char
    ['] N-pattern [char] N redefine-char
    ['] O-pattern [char] O redefine-char 
    ['] P-pattern [char] P redefine-char
    ['] Q-pattern [char] Q redefine-char
    ['] R-pattern [char] R redefine-char
    ['] S-pattern [char] S redefine-char
    ['] T-pattern [char] T redefine-char
    ['] U-pattern [char] U redefine-char 
    ['] V-pattern [char] V redefine-char
    ['] W-pattern [char] W redefine-char
    ['] X-pattern [char] X redefine-char
    ['] Y-pattern [char] Y redefine-char
    ['] Z-pattern [char] Z redefine-char ;

: clear-screen
    0 0x1800 768 fill-vram ;

variable cursor

: locate ( x y -- )
    32 * + 0x1800 + cursor ! ;

: 1+! ( addr -- )
    dup @ 1+ swap ! ;

: emit ( n -- )
    cursor @ vram! 
    cursor 1+! ;

: type ( c-addr +n -- )
    ?dup if
        over + swap do i c@ emit loop
    else drop then ;

: cr
    cursor @ 0b1111111111100000 and 32 + cursor ! ;

: show-message
    0 0 locate
    s" BY" type cr
    s" VICTOR MARZO" type ;

: init-screen
    0 0 15 change-color
    init-graph
    clear-screen
    redefine-tiles ;

: main
    disable-screen
    init-screen
    show-message
    enable-screen
    abort ;
