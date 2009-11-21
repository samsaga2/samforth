\ STACK OPERATIONS =========================================
ASM: DUP DUP ( x -- x x )
    push bc
;ASM

ASM: ?DUP QDUP ( x -- 0 | x x )
    ld a,b
    or c
    jr nz,DUP
;ASM

ASM: DROP DROP ( x -- )
    pop bc
;ASM

ASM: 2DROP TWODROP ( x y -- )
    pop bc
    pop bc
;ASM

ASM: 3DROP THREEDROP ( x y z -- )
    pop bc
    pop bc
    pop bc
;ASM

ASM: 4DROP FOURDROP ( x y z w -- )
    pop bc
    pop bc
    pop bc
    pop bc
;ASM

ASM: SWAP SWAP ( x y -- y x )
    pop hl
    push bc
    ld b,h
    ld c,l
;ASM

ASM: OVER OVER ( x y -- x y x )
    pop hl
    push hl
    push bc
    ld b,h
    ld c,l
;ASM

ASM: ROT ROT ( x y z -- y z x )
    pop hl \ y
    ex (sp),hl \ y n stack, x in hl
    push bc
    ld b,h
    ld c,l
;ASM

ASM: >R TOR ( x-- ) ( R: -- x )
    dec ix \ push TOS into RST
    ld (ix+0),b
    dec ix
    ld (ix+0),c
    pop bc \ pop new TOS
;ASM

ASM: R> RFROM ( -- x ) ( R: x -- )
    push bc \ push TOS
    ld c,(ix+0) \ pop RST to TOS
    inc ix
    ld b,(ix+0)
    inc ix
;ASM

ASM: R@ RFETCH ( -- x ) ( R: x -- x )
    push bc \ push TOS
    ld c,(ix+0) \ fetch RST to TOS
    ld b,(ix+1)
;ASM

: NIP ( x y -- y ) SWAP DROP ;
: TUCK ( x y -- y x y ) SWAP OVER ;

\ MEMORY OPERATIONS ========================================
ASM: ! STORE ( n addr -- )
    ld h,b \ address in hl
    ld l,c
    pop bc \ data in bc
    ld (hl),c
    inc hl
    ld (hl),b
    pop bc \ pop TOS
;ASM

ASM: C! CSTORE ( char addr -- )
    ld h,b \ addres in hl
    ld l,c
    pop bc \ data in bc
    ld (hl),c
    pop bc \ pop TOS
;ASM

ASM: @ FETCH ( addr -- n )
    ld h,b \ address in hl
    ld l,c
    ld c,(hl)
    inc hl
    ld b,(hl)
;ASM

ASM: C@ CFETCH ( addr -- char )
    ld a,(bc)
    ld c,a
    ld b,0
;ASM

ASM: PC! PCSTORE ( char port -- )
    pop hl \ char in L
    out (c),l \ to port (c)
    pop bc \ pop TOS
;ASM

ASM: PC@ PCFETCH ( port -- char )
    in c,(c) \ read port (c) to c
    ld b,0
;ASM

\ ARITHMETIC AND LOGICAL OPERATIONS ========================
ASM: + PLUS ( n1/u1 n2/u2 -- n3/u3 )
    pop hl
    add hl,bc
    ld b,h
    ld c,l
;ASM

ASM: - MINUS ( n1/u1 n2/u2 -- n3/u3 )
    pop hl
    or a
    sbc hl,bc
    ld b,h
    ld c,l
;ASM

ASM: AND AND ( x y -- x )
    pop hl
    ld a,b
    and h
    ld b,a
    ld a,c
    and l
    ld c,a
;ASM

ASM: OR OR ( x y -- x )
    pop hl
    ld a,b
    or h
    ld b,a
    ld a,c
    or l
    ld c,a
;ASM

ASM: XOR XOR ( x y -- x )
    pop hl
    ld a,b
    xor h
    ld b,a
    ld a,c
    xor l
    ld c,a
;ASM

ASM: INVERT INVERT ( x -- y )
    ld a,b
    cpl
    ld b,a
    ld a,c
    cpl
    ld c,a
;ASM

ASM: NEGATE NEGATE ( x -- y )
    ld a,b
    cpl
    ld b,a
    ld a,c
    cpl
    ld c,a
    inc bc
;ASM

ASM: 1+ ONEPLUS ( n1/u1 -- n2/u2 )
    inc bc
;ASM

ASM: 1- ONEMINUS ( n1/u1 -- n2/u2 )
    dec bc
;ASM

ASM: 2* TWOSTAR ( x -- y )
    sla c
    rl b
;ASM

ASM: 2/ TWOSLASH ( x -- y )
    sra b
    rr c
;ASM

ASM: LSHIFT LSHIFT ( x u -- y )
    ld b,c \ b=loop counter
    pop hl
    inc b \ test for counter=0
    jr .lsh2
.lsh1:
    add hl,hl \ left shift HL, n times
.lsh2:
    djnz .lsh1
    ld b,h \ result
    ld c,l
;ASM

ASM: RSHIFT RSHIFT ( x u -- y )
    ld b,c			\ b=loop counter
    pop hl
    inc b			\ test for counter=0
    jr .rsh2
.rsh1:
    srl h			\ right shift HL, n times
    rr l
.rsh2:
    djnz .rsh1
    ld b,h			\ result
    ld c,l
;ASM

ASM: +! PLUSSTORE ( n/u a-addr -- )
    pop hl
    ld a,(bc)		\ low byte
    add a,l
    ld (bc),a
    inc bc
    ld a,(bc)		\ high byte
    adc a,h
    ld (bc),a
    pop bc			\ pop TOS
;ASM

\ COMPARISION OPERATIONS ===================================
ASM: 0= ZEROEQUAL ( n -- flag )
    ld a,b
    or c                    \ result=0 if bc was 0
    sub 1                   \ cy set   if was 0
    sbc a,a                 \ propagate cy through A
    ld b,a                  \ oput 0000 or FFFF in TOS
    ld c,a
;ASM

ASM: 0< ZEROLES ( n -- flag )
    sla b                   \ sign bit -> cy flag
    sbc a,a                 \ propagate cy through A
    ld b,a                  \ put 0000 or FFFF in TOS
    ld c,a
;ASM

ASM: = EQUAL ( x y -- flag )
    pop hl
    or a
    sbc hl,bc               \ x1-x2 in HL, SZVC valid
    jr z,tostrue
tosfalse:
    ld bc,0
    NEXT
tostrue:
    ld bc,0ffffh
;ASM

ASM: < LESS ( x y -- flag )
    pop hl
    or a
    sbc hl,bc
    jp pe,.revsense
    jp p,tosfalse
    jp tostrue
.revsense:
    jp m,tosfalse
    ld bc,0ffffh
;ASM

ASM: U< ULESS ( u1 u2 -- flag )
    pop hl
    or a
    sbc hl,bc
    sbc a,a
    ld b,a
    ld c,a
;ASM

: <> ( x y -- flag ) = 0= ;
: > ( x y -- flag) SWAP < ;
: U> ( x y -- flag ) SWAP U< ;

\ LOOP AND BRANCH OPERATIONS ===============================

ASM: BRANCH BRANCH
    ld a,(de)     		\  get inline value => IP
    ld l,a
    inc de
    ld a,(de)
    ld h,a
;ASMHL

ASM: ?BRANCH QBRANCH ( x -- )
        ld a,b
        or c            	\ test old TOS
        pop bc          	\ pop new TOS
        jr z,BRANCH		\ if old TOS=0, branch
        inc de          	\ else skip inline value
        inc de
;ASM

ASM: (do) XDO ( n1|u1 n2|u2 --	R: -- sys1 sys2 )
    ex de,hl
    ex (sp),hl   		\ IP on stack, limit in HL
    ex de,hl
    ld hl,8000h
    or a
    sbc hl,de    		\ 8000-limit in HL
    dec ix       		\ push this fudge factor
    ld (ix+0),h  		\    onto return stack
    dec ix       		\    for later use by 'I'
    ld (ix+0),l
    add hl,bc    		\ add fudge to start value
    dec ix       		\ push adjusted start value
    ld (ix+0),h  		\    onto return stack
    dec ix       		\    as the loop index.
    ld (ix+0),l
    pop de       		\ restore the saved IP
    pop bc       		\ pop new TOS
;ASM

ASM: (loop) XLOOP ( R: sys1 sys2 -- | sys1 sys2 )
    exx
    ld bc,1
looptst:
    ld l,(ix+0)  	\ get the loop index
    ld h,(ix+1)
    or a
    adc hl,bc    	\ increment w/overflow test
    jp pe,loopterm  	\ overflow=loop done
    ld (ix+0),l  	\ save the updated index
    ld (ix+1),h
    exx
    jr BRANCH		\ take the inline branch
loopterm: 		\ terminate the loop
    ld bc,4      	\ discard the loop info
    add ix,bc
    exx
    inc de       	\ skip the inline branch
    inc de
;ASM

ASM: (+loop) XPLUSLOOP ( n -- ) ( R: sys1 sys2 -- | sys1 sys2 )
    pop hl      		\ this will be the new TOS
    push bc
    ld b,h
    ld c,l
    exx
    pop bc      		\ old TOS = loop increment
    jr looptst
;ASM

ASM: I I ( -- n ) ( R: sys1 sys2 -- sys1 sys2 )
    push bc     		\ push old TOS
    ld l,(ix+0) 		\ get current loop index
    ld h,(ix+1)
    ld c,(ix+2) 		\ get fudge factor
    ld b,(ix+3)
    or a
    sbc hl,bc   		\ subtract fudge factor,
    ld b,h      		\   returning true index
    ld c,l
;ASM

ASM: J J ( -- n) ( R: 4*sys -- 4*sys )
    push bc     		\ push old TOS
    ld l,(ix+4) 		\ get current loop index
    ld h,(ix+5)
    ld c,(ix+6) 		\ get fudge factor
    ld b,(ix+7)
    or a
    sbc hl,bc   		\ subtract fudge factor,
    ld b,h      		\   returning true index
    ld c,l
;ASM

ASM: UNLOOP UNLOOP ( -- ) ( R: sys1 sys2 -- )
    inc ix
    inc ix
    inc ix
    inc ix
;ASM

\ MULTIPLY AND DIVIDE ======================================
ASM: UM* UMSTAR ( u1 u2 - ud )
    push bc
    exx
    pop bc      \ u2 in BC
    pop de      \ u1 in DE
    ld hl,0     \ result will be in HLDE
    ld a,17     \ loop counter
    or a        \ clear cy
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
    push de     \ lo result
    push hl     \ hi result
    exx
    pop bc      \ put TOS back in BC
;ASM

ASM: UM/MOD UMSLASHMOD ( ud u1 -- u2 u3 )
    push bc
    exx
    pop bc      \ BC = divisor
    pop hl      \ HLDE = dividend
    pop de
    ld a,16     \ loop counter
    sla e
    rl d        \ hi bit DE -> carry
udloop:
    adc hl,hl   \ rot left w/ carry
    jr nc,udiv3
    \ case 1: 17 bit, cy:HL = 1xxxx
    or a        \ we know we can subtract
    sbc hl,bc
    or a        \ clear cy to indicate sub ok
    jr udiv4
    \ case 2: 16 bit, cy:HL = 0xxxx
udiv3:
    sbc hl,bc   \ try the subtract
    jr nc,udiv4 \ if no cy, subtract ok
    add hl,bc   \ else cancel the subtract
    scf         \   and set cy to indicate
udiv4:
    rl e        \ rotate result bit into DE,
    rl d        \ and next bit of DE into cy
    dec a
    jr nz,udloop
    \ now have complemented quotient in DE,
    \ and remainder in HL
    ld a,d
    cpl
    ld b,a
    ld a,e
    cpl
    ld c,a
    push hl     \ push remainder
    push bc
    exx
    pop bc      \ quotient remains in TOS
;ASM

\ BLOCK AND STRING OPERATIONS ==============================
ASM: FILL FILL ( addr u char -- )
    ld a,c          \ character in a
    exx             \ use alt. register set
    pop bc          \ count in bc
    pop de          \ address in de
    or a            \ clear carry flag
    ld hl,0ffffh
    adc hl,bc       \ test for count=0 or 1
    jr nc,filldone  \   no cy: count=0, skip
    ld (de),a       \ fill first byte
    jr z,filldone   \   zero, count=1, done
    dec bc          \ else adjust count,
    ld h,d          \   let hl = start adrs,
    ld l,e
    inc de          \   let de = start adrs+1
    ldir            \   copy (hl)->(de)
filldone:
    exx             \ back to main reg set
    pop bc          \ pop new TOS
;ASM

ASM: CMOVE CMOVE ( c-addr1 c-addr2 u -- )
    push bc
    exx
    pop bc      \ count
    pop de      \ destination adrs
    pop hl      \ source adrs
    ld a,b      \ test for count=0
    or c
    jr z,cmovedone
    ldir        \ move from bottom to top
cmovedone:
    exx
    pop bc      \ pop new TOS
;ASM

ASM: CMOVE> CMOVEUP ( c-addr1 c-addr2 u --  )
    push bc
    exx
    pop bc      \ count
    pop hl      \ destination adrs
    pop de      \ source adrs
    ld a,b      \ test for count=0
    or c
    jr z,umovedone
    add hl,bc   \ last byte in destination
    dec hl
    ex de,hl
    add hl,bc   \ last byte in source
    dec hl
    lddr        \ move from top to bottom
umovedone:
    exx
    pop bc      \ pop new TOS
;ASM

ASM: SKIP SKIP ( c-addr u c -- c-addr' u' )
    ld a,c      \ skip character
    exx
    pop bc      \ count
    pop hl      \ address
    ld e,a      \ test for count=0
    ld a,b
    or c
    jr z,skipdone
    ld a,e
skiploop:
    cpi
    jr nz,skipmis   \ char mismatch: exit
    jp pe,skiploop  \ count not exhausted
    jr skipdone     \ count 0, no mismatch
skipmis:
    inc bc         \ mismatch!  undo last to
    dec hl          \  point at mismatch char
skipdone:
    push hl   \ updated address
    push bc     \ updated count
    exx
    pop bc      \ TOS in bc
;ASM

ASM: SCAN SCAN ( c-addr u c -- c-addr' u' )
    ld a,c      \ scan character
    exx
    pop bc      \ count
    pop hl      \ address
    ld e,a      \ test for count=0
    ld a,b
    or c
    jr z,scandone
    ld a,e
    cpir        \ scan 'til match or count=0
    jr nz,scandone  \ no match, BC & HL ok
    inc bc          \ match!  undo last to
    dec hl          \   point at match char
scandone:
    push hl   \ updated address
    push bc     \ updated count
    exx
    pop bc      \ TOS in bc
;ASM

ASM: S= SEQUAL ( c-addr1 c-addr2 u -- n )
    push bc
    exx
    pop bc      \ count
    pop hl      \ addr2
    pop de      \ addr1
    ld a,b      \ test for count=0
    or c
    jr z,smatch     \ by definition, match!
sloop:
    ld a,(de)
    inc de
    cpi
    jr nz,sdiff     \ char mismatch: exit
    jp pe,sloop     \ count not exhausted
smatch: \ count exhausted & no mismatch found
    exx
    ld bc,0         \ bc=0000  (s1=s2)
    jr snext
sdiff:  \ mismatch!  undo last 'cpi' increment
    dec hl          \ point at mismatch char
    cp (hl)         \ set cy if char1 < char2
    sbc a,a         \ propagate cy thru A
    exx
    ld b,a          \ bc=FFFF if cy (s1<s2)
    or 1            \ bc=0001 if ncy (s1>s2)
    ld c,a
snext:
;ASM

\ MISC OPERATIONS ==========================================

ASM: EMIT EMIT ( n -- )
    ld a,c
    pop bc
    exx
    call 00a2h
    exx
;ASM

: TYPE ( c-addr +n -- )
    ?DUP IF
        OVER + SWAP DO I C@ EMIT LOOP
    ELSE DROP THEN ;

: BL ( -- ) 32 EMIT ;
: CR ( -- ) 13 EMIT 10 EMIT ;

: ABORT ( -- ) RECURSE ;
