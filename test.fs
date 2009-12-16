INCLUDE kernel.fs
INCLUDE bios.fs

create star-pattern 
0b00000000 c,
0b00000000 c,
0b00000000 c,
0b00001111 c,
0b11110000 c,
0b00000000 c,
0b00000000 c,
0b00000000 c,
0x6f c,

: char-vaddr ( c -- v-addr )
    3 lshift ;

: char-color-vaddr ( c -- v-addr )
    3 rshift 0x2000 + ;

: bold-font ( -- )
    127 char-vaddr 7 +
    32 char-vaddr
    do
        i vram@ dup 1 rshift or i vram!
    loop ;

: redefine-char ( char addr -- )
    \ tile
    2dup swap char-vaddr swap 8 -rot ram-to-vram
    \ color
    8 + c@ swap char-color-vaddr vram! ;

: show-message
    ." **************************" cr
    ." SamForth " 2009 decimal . cr
    ." by Victor Marzo" cr
    ." <samsaga2@gmail.com>" cr
    ." **************************" cr cr ;

: init-screen
    32 SYS-LINL32 c!
    init-mode32 0 0 15 change-color
    bold-font
    [char] * ['] star-pattern redefine-char ;

: main
    init-screen show-message abort ;
