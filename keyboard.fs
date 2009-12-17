\ read the keyboard line status
asm: keyboard-status@
    ; ( line -- status )
    di
    in a,(0aah)
    and 0f0h
    add a,c
    out (0aah),a
    ei
    in a,(0a9h)
    ld b,0
    ld c,a
    ei
;asm

\ return 1 if the key is pressed
: key-status@ ( key-bit key-line -- status )
    keyboard-status@
    swap and
    if 0 else 1 then ;

: KEY-7     0b10000000 0 ;
: KEY-6     0b01000000 0 ;
: KEY-5     0b00100000 0 ;
: KEY-4     0b00010000 0 ;
: KEY-3     0b00001000 0 ;
: KEY-2     0b00000100 0 ;
: KEY-1     0b00000010 0 ;
: KEY-0     0b00000001 0 ;

: KEY-;     0b10000000 1 ;
: KEY-]     0b01000000 1 ;
: KEY-[     0b00100000 1 ;
: KEY-\     0b00010000 1 ;
: KEY-=     0b00001000 1 ;
: KEY--     0b00000100 1 ;
: KEY-9     0b00000010 1 ;
: KEY-8     0b00000001 1 ;

: KEY-B     0b10000000 2 ;
: KEY-A     0b01000000 2 ;
: KEY-???   0b00100000 2 ;
: KEY-/     0b00010000 2 ;
: KEY-.     0b00001000 2 ;
: KEY-,     0b00000100 2 ;
: KEY-'     0b00000010 2 ;
: KEY-`     0b00000001 2 ;

: KEY-J     0b10000000 3 ;
: KEY-I     0b01000000 3 ;
: KEY-H     0b00100000 3 ;
: KEY-G     0b00010000 3 ;
: KEY-F     0b00001000 3 ;
: KEY-E     0b00000100 3 ;
: KEY-D     0b00000010 3 ;
: KEY-C     0b00000001 3 ;

: KEY-R     0b10000000 4 ;
: KEY-Q     0b01000000 4 ;
: KEY-P     0b00100000 4 ;
: KEY-O     0b00010000 4 ;
: KEY-N     0b00001000 4 ;
: KEY-M     0b00000100 4 ;
: KEY-L     0b00000010 4 ;
: KEY-K     0b00000001 4 ;

: KEY-Z     0b10000000 5 ;
: KEY-Y     0b01000000 5 ;
: KEY-X     0b00100000 5 ;
: KEY-W     0b00010000 5 ;
: KEY-V     0b00001000 5 ;
: KEY-U     0b00000100 5 ;
: KEY-T     0b00000010 5 ;
: KEY-S     0b00000001 5 ;

: KEY-Z     0b10000000 6 ;
: KEY-Y     0b01000000 6 ;
: KEY-X     0b00100000 6 ;
: KEY-W     0b00010000 6 ;
: KEY-V     0b00001000 6 ;
: KEY-U     0b00000100 6 ;
: KEY-T     0b00000010 6 ;
: KEY-S     0b00000001 6 ;

: KEY-F3    0b10000000 7 ;
: KEY-F2    0b01000000 7 ;
: KEY-F1    0b00100000 7 ;
: KEY-CODE  0b00010000 7 ;
: KEY-CAP   0b00001000 7 ;
: KEY-GRAPH 0b00000100 7 ;
: KEY-CTRL  0b00000010 7 ;
: KEY-SHIFT 0b00000001 7 ;

: KEY-RIGHT 0b10000000 8 ;
: KEY-DOWN  0b01000000 8 ;
: KEY-UP    0b00100000 8 ;
: KEY-LEFT  0b00010000 8 ;
: KEY-DEL   0b00001000 8 ;
: KEY-INS   0b00000100 8 ;
: KEY-HOME  0b00000010 8 ;
: KEY-SPACE 0b00000001 8 ;
