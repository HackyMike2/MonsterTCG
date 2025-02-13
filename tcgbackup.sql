PGDMP  :                     }            Monster_TCG    17.2    17.2 (    �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                           false            �           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                           false            �           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                           false            �           1262    16552    Monster_TCG    DATABASE     �   CREATE DATABASE "Monster_TCG" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'German_Austria.1252';
    DROP DATABASE "Monster_TCG";
                     postgres    false            �           0    0    DATABASE "Monster_TCG"    COMMENT     @   COMMENT ON DATABASE "Monster_TCG" IS 'my monster_TCG Database';
                        postgres    false    4840            �            1259    16554    card    TABLE     �   CREATE TABLE public.card (
    id integer NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Damage" integer NOT NULL,
    "Type" integer NOT NULL,
    "Species" integer NOT NULL
);
    DROP TABLE public.card;
       public         heap r       postgres    false            �            1259    16553    MonsterCard_ID_seq    SEQUENCE     �   CREATE SEQUENCE public."MonsterCard_ID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 +   DROP SEQUENCE public."MonsterCard_ID_seq";
       public               postgres    false    218            �           0    0    MonsterCard_ID_seq    SEQUENCE OWNED BY     D   ALTER SEQUENCE public."MonsterCard_ID_seq" OWNED BY public.card.id;
          public               postgres    false    217            �            1259    16571    tcguser    TABLE     �   CREATE TABLE public.tcguser (
    id integer NOT NULL,
    username character varying(100),
    password character varying(100),
    coins integer,
    elo integer,
    securitytoken character varying(100)
);
    DROP TABLE public.tcguser;
       public         heap r       postgres    false            �            1259    16570    User_ID_seq    SEQUENCE     �   CREATE SEQUENCE public."User_ID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 $   DROP SEQUENCE public."User_ID_seq";
       public               postgres    false    220            �           0    0    User_ID_seq    SEQUENCE OWNED BY     @   ALTER SEQUENCE public."User_ID_seq" OWNED BY public.tcguser.id;
          public               postgres    false    219            �            1259    16580 
   collection    TABLE     p   CREATE TABLE public.collection (
    userid integer NOT NULL,
    cardid integer NOT NULL,
    count integer
);
    DROP TABLE public.collection;
       public         heap r       postgres    false            �            1259    16577    deck    TABLE     E   CREATE TABLE public.deck (
    userid integer,
    cardid integer
);
    DROP TABLE public.deck;
       public         heap r       postgres    false            �            1259    16609    packcontents    TABLE     q   CREATE TABLE public.packcontents (
    packcontentid integer NOT NULL,
    packid integer,
    cardid integer
);
     DROP TABLE public.packcontents;
       public         heap r       postgres    false            �            1259    16608    packcontents_packcontentid_seq    SEQUENCE     �   CREATE SEQUENCE public.packcontents_packcontentid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 5   DROP SEQUENCE public.packcontents_packcontentid_seq;
       public               postgres    false    226            �           0    0    packcontents_packcontentid_seq    SEQUENCE OWNED BY     a   ALTER SEQUENCE public.packcontents_packcontentid_seq OWNED BY public.packcontents.packcontentid;
          public               postgres    false    225            �            1259    16604    packs    TABLE     n   CREATE TABLE public.packs (
    packid integer NOT NULL,
    name character varying(100),
    cost integer
);
    DROP TABLE public.packs;
       public         heap r       postgres    false            �            1259    16603    packs_packid_seq    SEQUENCE     �   CREATE SEQUENCE public.packs_packid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 '   DROP SEQUENCE public.packs_packid_seq;
       public               postgres    false    224            �           0    0    packs_packid_seq    SEQUENCE OWNED BY     E   ALTER SEQUENCE public.packs_packid_seq OWNED BY public.packs.packid;
          public               postgres    false    223            8           2604    16557    card id    DEFAULT     k   ALTER TABLE ONLY public.card ALTER COLUMN id SET DEFAULT nextval('public."MonsterCard_ID_seq"'::regclass);
 6   ALTER TABLE public.card ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    218    217    218            ;           2604    16612    packcontents packcontentid    DEFAULT     �   ALTER TABLE ONLY public.packcontents ALTER COLUMN packcontentid SET DEFAULT nextval('public.packcontents_packcontentid_seq'::regclass);
 I   ALTER TABLE public.packcontents ALTER COLUMN packcontentid DROP DEFAULT;
       public               postgres    false    225    226    226            :           2604    16607    packs packid    DEFAULT     l   ALTER TABLE ONLY public.packs ALTER COLUMN packid SET DEFAULT nextval('public.packs_packid_seq'::regclass);
 ;   ALTER TABLE public.packs ALTER COLUMN packid DROP DEFAULT;
       public               postgres    false    223    224    224            9           2604    16574 
   tcguser id    DEFAULT     g   ALTER TABLE ONLY public.tcguser ALTER COLUMN id SET DEFAULT nextval('public."User_ID_seq"'::regclass);
 9   ALTER TABLE public.tcguser ALTER COLUMN id DROP DEFAULT;
       public               postgres    false    220    219    220            �          0    16554    card 
   TABLE DATA           G   COPY public.card (id, "Name", "Damage", "Type", "Species") FROM stdin;
    public               postgres    false    218   �*       �          0    16580 
   collection 
   TABLE DATA           ;   COPY public.collection (userid, cardid, count) FROM stdin;
    public               postgres    false    222   �+       �          0    16577    deck 
   TABLE DATA           .   COPY public.deck (userid, cardid) FROM stdin;
    public               postgres    false    221   �+       �          0    16609    packcontents 
   TABLE DATA           E   COPY public.packcontents (packcontentid, packid, cardid) FROM stdin;
    public               postgres    false    226    ,       �          0    16604    packs 
   TABLE DATA           3   COPY public.packs (packid, name, cost) FROM stdin;
    public               postgres    false    224   P,       �          0    16571    tcguser 
   TABLE DATA           T   COPY public.tcguser (id, username, password, coins, elo, securitytoken) FROM stdin;
    public               postgres    false    220   �,       �           0    0    MonsterCard_ID_seq    SEQUENCE SET     C   SELECT pg_catalog.setval('public."MonsterCard_ID_seq"', 11, true);
          public               postgres    false    217            �           0    0    User_ID_seq    SEQUENCE SET     <   SELECT pg_catalog.setval('public."User_ID_seq"', 10, true);
          public               postgres    false    219            �           0    0    packcontents_packcontentid_seq    SEQUENCE SET     M   SELECT pg_catalog.setval('public.packcontents_packcontentid_seq', 12, true);
          public               postgres    false    225            �           0    0    packs_packid_seq    SEQUENCE SET     >   SELECT pg_catalog.setval('public.packs_packid_seq', 4, true);
          public               postgres    false    223            =           2606    16559    card MonsterCard_pkey 
   CONSTRAINT     U   ALTER TABLE ONLY public.card
    ADD CONSTRAINT "MonsterCard_pkey" PRIMARY KEY (id);
 A   ALTER TABLE ONLY public.card DROP CONSTRAINT "MonsterCard_pkey";
       public                 postgres    false    218            A           2606    16584    collection UserCards 
   CONSTRAINT     `   ALTER TABLE ONLY public.collection
    ADD CONSTRAINT "UserCards" PRIMARY KEY (userid, cardid);
 @   ALTER TABLE ONLY public.collection DROP CONSTRAINT "UserCards";
       public                 postgres    false    222    222            ?           2606    16576    tcguser User_pkey 
   CONSTRAINT     Q   ALTER TABLE ONLY public.tcguser
    ADD CONSTRAINT "User_pkey" PRIMARY KEY (id);
 =   ALTER TABLE ONLY public.tcguser DROP CONSTRAINT "User_pkey";
       public                 postgres    false    220            E           2606    16614    packcontents packcontents_pkey 
   CONSTRAINT     g   ALTER TABLE ONLY public.packcontents
    ADD CONSTRAINT packcontents_pkey PRIMARY KEY (packcontentid);
 H   ALTER TABLE ONLY public.packcontents DROP CONSTRAINT packcontents_pkey;
       public                 postgres    false    226            C           2606    16616    packs packs_pkey 
   CONSTRAINT     R   ALTER TABLE ONLY public.packs
    ADD CONSTRAINT packs_pkey PRIMARY KEY (packid);
 :   ALTER TABLE ONLY public.packs DROP CONSTRAINT packs_pkey;
       public                 postgres    false    224            F           2606    16585    collection Col_User    FK CONSTRAINT     �   ALTER TABLE ONLY public.collection
    ADD CONSTRAINT "Col_User" FOREIGN KEY (userid) REFERENCES public.tcguser(id) ON DELETE CASCADE NOT VALID;
 ?   ALTER TABLE ONLY public.collection DROP CONSTRAINT "Col_User";
       public               postgres    false    220    4671    222            G           2606    16590    collection col_Card    FK CONSTRAINT     �   ALTER TABLE ONLY public.collection
    ADD CONSTRAINT "col_Card" FOREIGN KEY (cardid) REFERENCES public.card(id) ON DELETE CASCADE NOT VALID;
 ?   ALTER TABLE ONLY public.collection DROP CONSTRAINT "col_Card";
       public               postgres    false    4669    222    218            �   �   x�=���0뽏Aw�혚W��"�V�pd"�|=8H���Y�B�c@��p��4]S�0&�>.�/7X�@Ƞ�q���hȢ+�VA`��1�pH��Òþ�a�u�����C9?CJ?͑�����E;_����!��C&�      �   0   x�%Ź  �x�Ļ�� ]b���L�w��*�"�����u����&      �   !   x�3�4�2�4bS.CNsa",�b���� E+$      �   @   x����0C�3�*�4�t�9�\�%l#�i��M���m�l�#��Z�!?��y��~�s
-      �   2   x�3�L�,*.)HL��44�2�.H��	 sM�L8�S���Rta�1z\\\ �SK      �   Q   x�3�,��M-���K��K-/H,..�/J�42�445@H��$���g��qpfg��%�&s�$�e��T�#D����qqq �� g     