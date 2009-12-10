INCLUDE kernel.fs
INCLUDE bios.fs

create star-pattern
binary
00000000 c,
00000000 c,
00000000 c,
00001111 c,
11110000 c,
00000000 c,
00000000 c,
00000000 c,
hex
6f c,
decimal

: char-vaddr ( c -- v-addr )
    3 lshift ;

: char-color-vaddr ( c -- v-addr )
    3 rshift [ hex ] 2000 [ decimal ] + ;

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
    32 sys-linl32 c!
    init-mode32 0 0 15 change-color
    bold-font
    [char] * star-pattern redefine-char ;

: main
    init-screen show-message abort ;
