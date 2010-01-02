variable r_seed

asm: (rnd16)
    ; ( seed -- n )
    push bc
    exx
    pop de
    ld a,d
    ld h,e
    ld l,253
    or a
    sbc hl,de
    sbc a,0
    sbc hl,de
    ld d,0
    sbc a,d
    ld e,a
    sbc hl,de
    jr nc,.rand
    inc hl
.rand:
    push hl
    exx
    pop bc
;asm

: rnd16 ( -- n )
  r_seed @ (rnd16) dup r_seed ! ;

: rnd8 ( -- n )
  rnd16 8 rshift ;

