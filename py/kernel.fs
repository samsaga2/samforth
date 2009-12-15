\ system variables =========================================

variable base
variable hp
128 array pad-buffer
variable pad

\ stack operations =========================================
asm: dup
    ; ( x -- x x )
    push bc
;asm

asm: ?dup
    ; ( x -- 0 | x x )
    ld a,b
    or c
    jr z,.end
    push bc
.end:
;asm

asm: drop
    ; ( x -- )
    pop bc
;asm

asm: 2drop
    ; ( x y -- )
    pop bc
    pop bc
;asm

asm: 3drop
    ; ( x y z -- )
    pop bc
    pop bc
    pop bc
;asm

asm: 4drop
    ; ( x y z w -- )
    pop bc
    pop bc
    pop bc
    pop bc
;asm

asm: swap
    ; ( x y -- y x )
    pop hl
    push bc
    ld b,h
    ld c,l
;asm

asm: over
    ; ( x y -- x y x )
    pop hl
    push hl
    push bc
    ld b,h
    ld c,l
;asm

asm: rot
    ; ( x y z -- y z x )
    pop hl ; y
    ex (sp),hl ; y in stack, x in hl
    push bc
    ld b,h
    ld c,l
;asm

: -rot ( x y z -- z x y )
    swap rot swap ;
    
asm: >r
    ; ( x-- ) ( r: -- x )
    dec ix ; push TOS into RST
    ld (ix+0),b
    dec ix
    ld (ix+0),c
    pop bc ; pop new TOS
;asm

asm: r>
    ; ( -- x ) ( r: x -- )
    push bc ; push tos
    ld c,(ix+0) ; pop RST to TOS
    inc ix
    ld b,(ix+0)
    inc ix
;asm

asm: r@
    ; ( -- x ) ( r: x -- x )
    push bc ; push tos
    ld c,(ix+0) ; fetch rst to TOS
    ld b,(ix+1)
;asm

: nip ( x y -- y ) swap drop ;
: tuck ( x y -- y x y ) swap over ;

\ memory operations ========================================
asm: !
    ; ( n addr -- )
    ld h,b ; address in hl
    ld l,c
    pop bc ; data in bc
    ld (hl),c
    inc hl
    ld (hl),b
    pop bc ; pop tos
;asm

asm: c!
    ; ( char addr -- )
    ld h,b ; addres in hl
    ld l,c
    pop bc ; data in bc
    ld (hl),c
    pop bc ; pop tos
;asm

asm: @
    ; ( addr -- n )
    ld h,b ; address in hl
    ld l,c
    ld c,(hl)
    inc hl
    ld b,(hl)
;asm

asm: c@
    ; ( addr -- char )
    ld a,(bc)
    ld c,a
    ld b,0
;asm

asm: pc!
    ; ( char port -- )
    pop hl ; char in l
    out (c),l ; to port (c)
    pop bc ; pop tos
;asm

asm: pc@
    ; ( port -- char )
    in c,(c) ; read port (c) to c
    ld b,0
;asm

\ arithmetic and logical operations ========================
asm: +
    ; ( n1/u1 n2/u2 -- n3/u3 )
    pop hl
    add hl,bc
    ld b,h
    ld c,l
;asm

asm: m+
    ; ( d n -- d )
    ex de,hl
    pop de ; hi cell
    ex (sp),hl ; lo cell, save ip
    add hl,bc
    ld b,d ; hi result in bc (tos)
    ld c,e
    jr nc,.mplus1
    inc bc
.mplus1:
    pop de ; restore saved ip
    push hl ; push lo result
;asm

asm: -
    ; ( n1/u1 n2/u2 -- n3/u3 )
    pop hl
    or a
    sbc hl,bc
    ld b,h
    ld c,l
;asm

asm: and
    ; ( x y -- x )
    pop hl
    ld a,b
    and h
    ld b,a
    ld a,c
    and l
    ld c,a
;asm

asm: or
    ; ( x y -- x )
    pop hl
    ld a,b
    or h
    ld b,a
    ld a,c
    or l
    ld c,a
;asm

asm: xor
    ; ( x y -- x )
    pop hl
    ld a,b
    xor h
    ld b,a
    ld a,c
    xor l
    ld c,a
;asm

asm: invert
    ; ( x -- y )
    ld a,b
    cpl
    ld b,a
    ld a,c
    cpl
    ld c,a
;asm

asm: negate
    ; ( x -- y )
    ld a,b
    cpl
    ld b,a
    ld a,c
    cpl
    ld c,a
    inc bc
;asm

asm: 1+
    ; ( n1/u1 -- n2/u2 )
oneplus:
    inc bc
;asm

asm: 1-
    ; ( n1/u1 -- n2/u2 )
    dec bc
;asm

asm: 2*
    ; ( x -- y )
twostar:
    sla c
    rl b
;asm

asm: 2/
    ; ( x -- y )
    sra b
    rr c
;asm

asm: lshift
    ; ( x u -- y )
    ld b,c ; b=loop counter
    pop hl
    inc b ; test for counter=0
    jr .lsh2
.lsh1:
    add hl,hl ; left shift hl, n times
.lsh2:
    djnz .lsh1
    ld b,h ; result
    ld c,l
;asm

asm: rshift
    ; ( x u -- y )
    ld b,c			; b=loop counter
    pop hl
    inc b			; test for counter=0
    jr .rsh2
.rsh1:
    srl h			; right shift hl, n times
    rr l
.rsh2:
    djnz .rsh1
    ld b,h			; result
    ld c,l
;asm

asm: +!
    ; ( n/u a-addr -- )
    pop hl
    ld a,(bc)		; low byte
    add a,l
    ld (bc),a
    inc bc
    ld a,(bc)		; high byte
    adc a,h
    ld (bc),a
    pop bc			; pop tos
;asm

\ comparision operations ===================================
asm: 0=
    ; ( n -- flag )
    ld a,b
    or c                    ; result=0 if bc was 0
    sub 1                   ; cy set   if was 0
    sbc a,a                 ; propagate cy through a
    ld b,a                  ; oput 0000 or ffff in tos
    ld c,a
;asm

asm: 0<
    ; ( n -- flag )
    sla b                   ; sign bit -> cy flag
    sbc a,a                 ; propagate cy through a
    ld b,a                  ; put 0000 or ffff in tos
    ld c,a
;asm

asm: =
    ; ( x y -- flag )
    pop hl
    or a
    sbc hl,bc               ; x1-x2 in hl, szvc valid
    jr z,tostrue
tosfalse:
    ld bc,0
    NEXT
tostrue:
    ld bc,0ffffh
;asm

asm: <
    ; ( x y -- flag )
    pop hl
    or a
    sbc hl,bc
    jp pe,.revsense
    jp p,tosfalse
    jp tostrue
.revsense:
    jp m,tosfalse
    ld bc,0ffffh
;asm

asm: u<
    ; ( u1 u2 -- flag )
    pop hl
    or a
    sbc hl,bc
    sbc a,a
    ld b,a
    ld c,a
;asm

: <> ( x y -- flag ) = 0= ;
: > ( x y -- flag ) swap < ;
: u> ( x y -- flag ) swap u< ;

\ loop and branch operations ===============================

asm: branch
branch:
    ld a,(de)     		;  get inline value => ip
    ld l,a
    inc de
    ld a,(de)
    ld h,a
;asmhl

asm: ?branch
    ; ( x -- )
    ld a,b
    or c                ; test old tos
    pop bc          	; pop new tos
    jr z,branch		; if old tos=0, branch
    inc de          	; else skip inline value
    inc de
;asm

asm: (do)
    ; ( n1|u1 n2|u2 --	r: -- sys1 sys2 )
    ex de,hl
    ex (sp),hl   		; ip on stack, limit in hl
    ex de,hl
    ld hl,8000h
    or a
    sbc hl,de    		; 8000-limit in hl
    dec ix       		; push this fudge factor
    ld (ix+0),h  		;    onto return stack
    dec ix       		;    for later use by 'i'
    ld (ix+0),l
    add hl,bc    		; add fudge to start value
    dec ix       		; push adjusted start value
    ld (ix+0),h  		;    onto return stack
    dec ix       		;    as the loop index.
    ld (ix+0),l
    pop de       		; restore the saved ip
    pop bc       		; pop new tos
;asm

asm: (loop)
    ; ( r: sys1 sys2 -- | sys1 sys2 )
    exx
    ld bc,1
looptst:
    ld l,(ix+0)  	; get the loop index
    ld h,(ix+1)
    or a
    adc hl,bc    	; increment w/overflow test
    jp pe,loopterm  	; overflow=loop done
    ld (ix+0),l  	; save the updated index
    ld (ix+1),h
    exx
    jr branch		; take the inline branch
loopterm: 		; terminate the loop
    ld bc,4      	; discard the loop info
    add ix,bc
    exx
    inc de       	; skip the inline branch
    inc de
;asm

asm: (+loop)
    ; ( n -- ) ( r: sys1 sys2 -- | sys1 sys2 )
    pop hl      		; this will be the new tos
    push bc
    ld b,h
    ld c,l
    exx
    pop bc      		; old tos = loop increment
    jr looptst
;asm

asm: i
    ; ( -- n )
    ; ( r: sys1 sys2 -- sys1 sys2 )
    push bc     		; push old tos
    ld l,(ix+0) 		; get current loop index
    ld h,(ix+1)
    ld c,(ix+2) 		; get fudge factor
    ld b,(ix+3)
    or a
    sbc hl,bc   		; subtract fudge factor,
    ld b,h      		;   returning true index
    ld c,l
;asm

asm: j
    ; ( -- n )
    ; ( r: 4*sys -- 4*sys )
    push bc     		; push old tos
    ld l,(ix+4) 		; get current loop index
    ld h,(ix+5)
    ld c,(ix+6) 		; get fudge factor
    ld b,(ix+7)
    or a
    sbc hl,bc   		; subtract fudge factor,
    ld b,h      		;   returning true index
    ld c,l
;asm

asm: unloop
    ; ( -- ) ( r: sys1 sys2 -- )
    inc ix
    inc ix
    inc ix
    inc ix
;asm

\ multiply and divide ======================================
asm: um*
    ; ( u1 u2 - ud )
    push bc
    exx
    pop bc      ; u2 in bc
    pop de      ; u1 in de
    ld hl,0     ; result will be in hlde
    ld a,17     ; loop counter
    or a        ; clear cy
umloop:
    rr h
    rr l
    rr d
    rr e
    jr nc,noadd
    add hl,bc
noadd:
    dec a
    jr nz,umloop
    push de     ; lo result
    push hl     ; hi result
    exx
    pop bc      ; put tos back in bc
;asm

asm: um/mod
    ; ( ud u1 -- u2 u3 )
    push bc
    exx
    pop bc      ; bc = divisor
    pop hl      ; hlde = dividend
    pop de
    ld a,16     ; loop counter
    sla e
    rl d        ; hi bit de -> carry
udloop:
    adc hl,hl   ; rot left w/ carry
    jr nc,udiv3
    ; case 1: 17 bit, cy:hl = 1xxxx
    or a        ; we know we can subtract
    sbc hl,bc
    or a        ; clear cy to indicate sub ok
    jr udiv4
    ; case 2: 16 bit, cy:hl = 0xxxx
udiv3:
    sbc hl,bc   ; try the subtract
    jr nc,udiv4 ; if no cy, subtract ok
    add hl,bc   ; else cancel the subtract
    scf         ;   and set cy to indicate
udiv4:
    rl e        ; rotate result bit into de,
    rl d        ; and next bit of de into cy
    dec a
    jr nz,udloop
    ; now have complemented quotient in de,
    ; and remainder in hl
    ld a,d
    cpl
    ld b,a
    ld a,e
    cpl
    ld c,a
    push hl     ; push remainder
    push bc
    exx
    pop bc      ; quotient remains in tos
;asm

\ block and string operations ==============================
asm: fill
    ; ( addr u char -- )
    ld a,c          ; character in a
    exx             ; use alt. register set
    pop bc          ; count in bc
    pop de          ; address in de
    or a            ; clear carry flag
    ld hl,0ffffh
    adc hl,bc       ; test for count=0 or 1
    jr nc,filldone  ;   no cy: count=0, skip
    ld (de),a       ; fill first byte
    jr z,filldone   ;   zero, count=1, done
    dec bc          ; else adjust count,
    ld h,d          ;   let hl = start adrs,
    ld l,e
    inc de          ;   let de = start adrs+1
    ldir            ;   copy (hl)->(de)
filldone:
    exx             ; back to main reg set
    pop bc          ; pop new tos
;asm

asm: cmove
    ; ( c-addr1 c-addr2 u -- )
    push bc
    exx
    pop bc      ; count
    pop de      ; destination adrs
    pop hl      ; source adrs
    ld a,b      ; test for count=0
    or c
    jr z,cmovedone
    ldir        ; move from bottom to top
cmovedone:
    exx
    pop bc      ; pop new tos
;asm

asm: cmove>
    ; ( c-addr1 c-addr2 u --  )
    push bc
    exx
    pop bc      ; count
    pop hl      ; destination adrs
    pop de      ; source adrs
    ld a,b      ; test for count=0
    or c
    jr z,umovedone
    add hl,bc   ; last byte in destination
    dec hl
    ex de,hl
    add hl,bc   ; last byte in source
    dec hl
    lddr        ; move from top to bottom
umovedone:
    exx
    pop bc      ; pop new tos
;asm

asm: skip
    ; ( c-addr u c -- c-addr' u' )
    ld a,c      ; skip character
    exx
    pop bc      ; count
    pop hl      ; address
    ld e,a      ; test for count=0
    ld a,b
    or c
    jr z,skipdone
    ld a,e
skiploop:
    cpi
    jr nz,skipmis   ; char mismatch: exit
    jp pe,skiploop  ; count not exhausted
    jr skipdone     ; count 0, no mismatch
skipmis:
    inc bc         ; mismatch!  undo last to
    dec hl          ;  point at mismatch char
skipdone:
    push hl   ; updated address
    push bc     ; updated count
    exx
    pop bc      ; tos in bc
;asm

asm: scan
    ; ( c-addr u c -- c-addr' u' )
    ld a,c      ; scan character
    exx
    pop bc      ; count
    pop hl      ; address
    ld e,a      ; test for count=0
    ld a,b
    or c
    jr z,scandone
    ld a,e
    cpir        ; scan 'til match or count=0
    jr nz,scandone  ; no match, bc & hl ok
    inc bc          ; match!  undo last to
    dec hl          ;   point at match char
scandone:
    push hl   ; updated address
    push bc     ; updated count
    exx
    pop bc      ; tos in bc
;asm

asm: s=
    ; ( c-addr1 c-addr2 u -- n )
    push bc
    exx
    pop bc      ; count
    pop hl      ; addr2
    pop de      ; addr1
    ld a,b      ; test for count=0
    or c
    jr z,smatch     ; by definition, match!
sloop:
    ld a,(de)
    inc de
    cpi
    jr nz,sdiff     ; char mismatch: exit
    jp pe,sloop     ; count not exhausted
smatch: ; count exhausted & no mismatch found
    exx
    ld bc,0         ; bc=0000  (s1=s2)
    jr snext
sdiff:  ; mismatch!  undo last 'cpi' increment
    dec hl          ; point at mismatch char
    cp (hl)         ; set cy if char1 < char2
    sbc a,a         ; propagate cy thru a
    exx
    ld b,a          ; bc=ffff if cy (s1<s2)
    or 1            ; bc=0001 if ncy (s1>s2)
    ld c,a
snext:
;asm

\ constants ================================================

asm: s0
    ; ( -- addr )
    push bc
    ld bc,PSP
;asm

asm: r0
    ; ( -- addr )
    push bc
    ld bc,RSP
;asm

\ alignment and portability operators ======================

: cell ( -- n ) 2 ;

asm: cell+
    ; ( addr1 -- addr2 )
    inc bc
    inc bc
;asm

asm: cells
    ; ( n1 -- n2 )
    jp twostar
;asm

asm: char+
    ; ( c-addr1 -- c-addr2 )
    jp oneplus
;asm

asm: chars
    ; ( n1 -- n2 )
;asm

\ double operators =========================================

: 2@ ( addr -- x1 x2 )
    dup cell+ @ swap @ ;

: 2! ( x1 x2 addr -- )
    swap over ! cell+ ! ;

: 2drop ( x1 x2 -- )
    drop drop ;

: 2dup ( x1 x2 -- x1 x2 x1 x2 )
    over over ;

: 2swap ( x1 x2 x3 x4 -- x3 x4 x1 x2 )
    rot >r rot r> ;

: 2over ( x1 x2 x3 x4 -- x1 x2 x3 x4 x1 x2 )
    >r >r 2dup r> r> 2swap ;

\ arithmetic operators =====================================

: s>d ( n -- d )
    dup 0< ;

: ?negate ( n1 n2 -- n3 )
    0< if negate then ;

: abs ( n1 -- +n2 )
    dup ?negate ;

: dnegate ( d1 -- d2 )
    swap invert swap invert 1 m+ ;

: ?dnegate ( d1 n -- d2 )
    0< if dnegate then ;

: dabs ( d1 -- +d2 )
    dup ?dnegate ;

: m* ( n1 n2 -- d )
    2dup xor >r
    swap abs swap abs um*
    r> ?dnegate ;

: sm/rem ( d1 n1 -- n2 n3 )
    2dup xor >r
    over >r
    abs >r dabs r> um/mod
    swap r> ?negate
    swap r> ?negate ;

: fm/mod ( d1 n1 -- n2 n3 )
    dup >r
    sm/rem
    dup 0< if
        swap r> +
        swap 1-
    else r> drop then ;

: * ( n1 n2 -- n3 )
    m* drop ;

: /mod ( n1 n2 -- n3 n4 )
    >r s>d r> fm/mod ;

: / ( n1 n2 -- n3 )
    /mod nip ;

: mod ( n1 n2 -- n3 )
    /mod drop ;

: */mod ( n1 n2 n3 -- n4 n5 )
    >r m* r> fm/mod ;

: */ ( n1 n2 n3 -- n4 )
    */mod nip ;

: max ( n1 n2 -- n3 )
    2dup < if swap then drop ;

: min ( n1 n2 -- n3 )
    2dup > if swap then drop ;

\ input/output =============================================

: count ( c-addr1 -- c-addr2 u counted->adr/len )
    dup char+ swap c@ ;
 
asm: emit
    ; ( n -- )
    ld a,c
    pop bc
    exx
    call 00a2h
    exx
;asm

: cr ( -- ) 13 emit 10 emit ;

: bl ( -- ) 32 ;

: space ( -- ) bl emit ;

: spaces ( n -- )
    begin dup while space 1- repeat drop ;

: umin ( u1 u2 -- u )
    2dup u> if swap then drop ;

: umax ( u1 u2 -- u )
    2dup u< if swap then drop ;

: type ( c-addr +n -- )
    ?dup if
        over + swap do i c@ emit loop
    else drop then ;
    
\ numeric output ===========================================

: ud/mod ( ud1 u2 -- u3 ud4 )
    >r 0 r@ um/mod rot rot r> um/mod rot ;

: ud* ( ud1 d2 -- ud3 )
    dup >r um* drop swap r> um* rot + ;

: hold ( char -- )
    -1 hp +! hp @ c! ;

: <# ( -- )
    pad hp ! ;

hex

: >digit ( n -- c )
    dup 9 > 7 and + 30 + ;

: # ( ud1 -- ud2 )
    base @ ud/mod rot >digit hold ;

: #s ( ud1 -- ud2 )
    begin # 2dup or 0= until ;

: #> ( ud1 -- c-addr u )
    2drop hp @ pad over - ;

: sign ( n -- )
    0< if 2d hold then ;

: u. ( u -- )
    <# 0 #s #> type space ;

: . ( n -- )
    <# dup abs 0 #s rot sign #> type space ;

decimal

c: decimal ( -- )
    10 base ! ;c

c: hex ( -- )
    16 base ! ;c

c: binary ( -- )
    2 base ! ;c

c: octal ( -- )
    8 base ! ;c

\ misc operations ==========================================

: abort ( -- ) recurse ;

asm: SYSCALL
	;; SYSCALL	af bc de hl address -- af bc de hl
SYSCALL:
    ld (ix-1),e ; save de reg
    ld (ix-2),d
    pop hl ; hl reg
    pop de ; de reg
    pop iy ; bc reg
    pop af ; af reg
    push ix ; save ix reg
    ld ix,.return ; push return address
    push ix
    push bc ; push addr
    push iy ; bc=iy
    pop bc
    ret
.return:
    pop ix ; restore ix reg
    push af ; push regs to psp
    push bc
    push de
    ld b,h
    ld c,l
    ld e,(ix-1) ; restore de reg
    ld d,(ix-2)
;asm
