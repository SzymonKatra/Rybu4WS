// Generated from ../Rybu4WS.g4 by ANTLR 4.9.2
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.misc.*;
import org.antlr.v4.runtime.tree.*;
import java.util.List;
import java.util.Iterator;
import java.util.ArrayList;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast"})
public class Rybu4WSParser extends Parser {
	static { RuntimeMetaData.checkVersion("4.9.2", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		DOT=1, LBRACE=2, RBRACE=3, LPAREN=4, RPAREN=5, COLON=6, COMMA=7, SEMICOLON=8, 
		VAR=9, ACTION_CONDITION=10, ACTION_ARROW=11, VAR_RANGE=12, ASSIGNMENT=13, 
		OPERATOR_INCREMENT=14, OPERATOR_DECREMENT=15, CONDITION_EQUAL=16, CONDITION_NOT_EQUAL=17, 
		CONDITION_GREATER_THAN=18, CONDITION_LESS_THAN=19, CONDITION_GREATER_OR_EQUAL_THAN=20, 
		CONDITION_LESS_OR_EQUAL_THAN=21, CONDITION_AND=22, CONDITION_OR=23, MATCH_SKIP=24, 
		MATCH=25, RETURN=26, SERVER=27, PROCESS=28, NUMBER=29, ID=30, WHITESPACE=31;
	public static final int
		RULE_file = 0, RULE_process_declaration = 1, RULE_instance_declaration = 2, 
		RULE_instance_arguments = 3, RULE_instance_states = 4, RULE_instance_state_init = 5, 
		RULE_server_declaration = 6, RULE_argument_with_type_list = 7, RULE_argument_with_type = 8, 
		RULE_variable_declaration = 9, RULE_variable_type_integer = 10, RULE_variable_type_enum = 11, 
		RULE_action_declaration = 12, RULE_action_condition = 13, RULE_condition = 14, 
		RULE_condition_logic_operator = 15, RULE_condition_comparison_operator = 16, 
		RULE_statement = 17, RULE_statement_call = 18, RULE_statement_match = 19, 
		RULE_statement_match_option = 20, RULE_statement_state_mutation = 21, 
		RULE_statement_state_mutation_operator = 22, RULE_statement_return = 23, 
		RULE_enum_value = 24;
	private static String[] makeRuleNames() {
		return new String[] {
			"file", "process_declaration", "instance_declaration", "instance_arguments", 
			"instance_states", "instance_state_init", "server_declaration", "argument_with_type_list", 
			"argument_with_type", "variable_declaration", "variable_type_integer", 
			"variable_type_enum", "action_declaration", "action_condition", "condition", 
			"condition_logic_operator", "condition_comparison_operator", "statement", 
			"statement_call", "statement_match", "statement_match_option", "statement_state_mutation", 
			"statement_state_mutation_operator", "statement_return", "enum_value"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, "'.'", "'{'", "'}'", "'('", "')'", "':'", "','", "';'", "'var'", 
			"'|'", "'->'", "'..'", "'='", "'+='", "'-='", "'=='", "'!='", "'>'", 
			"'<'", "'>='", "'<='", "'&&'", "'||'", "'skip'", "'match'", "'return'", 
			"'server'", "'process'"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, "DOT", "LBRACE", "RBRACE", "LPAREN", "RPAREN", "COLON", "COMMA", 
			"SEMICOLON", "VAR", "ACTION_CONDITION", "ACTION_ARROW", "VAR_RANGE", 
			"ASSIGNMENT", "OPERATOR_INCREMENT", "OPERATOR_DECREMENT", "CONDITION_EQUAL", 
			"CONDITION_NOT_EQUAL", "CONDITION_GREATER_THAN", "CONDITION_LESS_THAN", 
			"CONDITION_GREATER_OR_EQUAL_THAN", "CONDITION_LESS_OR_EQUAL_THAN", "CONDITION_AND", 
			"CONDITION_OR", "MATCH_SKIP", "MATCH", "RETURN", "SERVER", "PROCESS", 
			"NUMBER", "ID", "WHITESPACE"
		};
	}
	private static final String[] _SYMBOLIC_NAMES = makeSymbolicNames();
	public static final Vocabulary VOCABULARY = new VocabularyImpl(_LITERAL_NAMES, _SYMBOLIC_NAMES);

	/**
	 * @deprecated Use {@link #VOCABULARY} instead.
	 */
	@Deprecated
	public static final String[] tokenNames;
	static {
		tokenNames = new String[_SYMBOLIC_NAMES.length];
		for (int i = 0; i < tokenNames.length; i++) {
			tokenNames[i] = VOCABULARY.getLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = VOCABULARY.getSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}
	}

	@Override
	@Deprecated
	public String[] getTokenNames() {
		return tokenNames;
	}

	@Override

	public Vocabulary getVocabulary() {
		return VOCABULARY;
	}

	@Override
	public String getGrammarFileName() { return "Rybu4WS.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public ATN getATN() { return _ATN; }

	public Rybu4WSParser(TokenStream input) {
		super(input);
		_interp = new ParserATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	public static class FileContext extends ParserRuleContext {
		public TerminalNode EOF() { return getToken(Rybu4WSParser.EOF, 0); }
		public List<Server_declarationContext> server_declaration() {
			return getRuleContexts(Server_declarationContext.class);
		}
		public Server_declarationContext server_declaration(int i) {
			return getRuleContext(Server_declarationContext.class,i);
		}
		public List<Instance_declarationContext> instance_declaration() {
			return getRuleContexts(Instance_declarationContext.class);
		}
		public Instance_declarationContext instance_declaration(int i) {
			return getRuleContext(Instance_declarationContext.class,i);
		}
		public List<Process_declarationContext> process_declaration() {
			return getRuleContexts(Process_declarationContext.class);
		}
		public Process_declarationContext process_declaration(int i) {
			return getRuleContext(Process_declarationContext.class,i);
		}
		public FileContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_file; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).enterFile(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).exitFile(this);
		}
	}

	public final FileContext file() throws RecognitionException {
		FileContext _localctx = new FileContext(_ctx, getState());
		enterRule(_localctx, 0, RULE_file);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(53);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==SERVER) {
				{
				{
				setState(50);
				server_declaration();
				}
				}
				setState(55);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(59);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==VAR) {
				{
				{
				setState(56);
				instance_declaration();
				}
				}
				setState(61);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(65);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==PROCESS) {
				{
				{
				setState(62);
				process_declaration();
				}
				}
				setState(67);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(68);
			match(EOF);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Process_declarationContext extends ParserRuleContext {
		public TerminalNode PROCESS() { return getToken(Rybu4WSParser.PROCESS, 0); }
		public TerminalNode LBRACE() { return getToken(Rybu4WSParser.LBRACE, 0); }
		public TerminalNode RBRACE() { return getToken(Rybu4WSParser.RBRACE, 0); }
		public List<StatementContext> statement() {
			return getRuleContexts(StatementContext.class);
		}
		public StatementContext statement(int i) {
			return getRuleContext(StatementContext.class,i);
		}
		public Process_declarationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_process_declaration; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).enterProcess_declaration(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).exitProcess_declaration(this);
		}
	}

	public final Process_declarationContext process_declaration() throws RecognitionException {
		Process_declarationContext _localctx = new Process_declarationContext(_ctx, getState());
		enterRule(_localctx, 2, RULE_process_declaration);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(70);
			match(PROCESS);
			setState(71);
			match(LBRACE);
			setState(75);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << MATCH) | (1L << RETURN) | (1L << ID))) != 0)) {
				{
				{
				setState(72);
				statement();
				}
				}
				setState(77);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(78);
			match(RBRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Instance_declarationContext extends ParserRuleContext {
		public TerminalNode VAR() { return getToken(Rybu4WSParser.VAR, 0); }
		public List<TerminalNode> ID() { return getTokens(Rybu4WSParser.ID); }
		public TerminalNode ID(int i) {
			return getToken(Rybu4WSParser.ID, i);
		}
		public TerminalNode ASSIGNMENT() { return getToken(Rybu4WSParser.ASSIGNMENT, 0); }
		public TerminalNode LPAREN() { return getToken(Rybu4WSParser.LPAREN, 0); }
		public TerminalNode RPAREN() { return getToken(Rybu4WSParser.RPAREN, 0); }
		public TerminalNode SEMICOLON() { return getToken(Rybu4WSParser.SEMICOLON, 0); }
		public Instance_argumentsContext instance_arguments() {
			return getRuleContext(Instance_argumentsContext.class,0);
		}
		public Instance_statesContext instance_states() {
			return getRuleContext(Instance_statesContext.class,0);
		}
		public Instance_declarationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_instance_declaration; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).enterInstance_declaration(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).exitInstance_declaration(this);
		}
	}

	public final Instance_declarationContext instance_declaration() throws RecognitionException {
		Instance_declarationContext _localctx = new Instance_declarationContext(_ctx, getState());
		enterRule(_localctx, 4, RULE_instance_declaration);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(80);
			match(VAR);
			setState(81);
			match(ID);
			setState(82);
			match(ASSIGNMENT);
			setState(83);
			match(ID);
			setState(84);
			match(LPAREN);
			setState(86);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==ID) {
				{
				setState(85);
				instance_arguments();
				}
			}

			setState(88);
			match(RPAREN);
			setState(90);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==LBRACE) {
				{
				setState(89);
				instance_states();
				}
			}

			setState(92);
			match(SEMICOLON);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Instance_argumentsContext extends ParserRuleContext {
		public List<TerminalNode> ID() { return getTokens(Rybu4WSParser.ID); }
		public TerminalNode ID(int i) {
			return getToken(Rybu4WSParser.ID, i);
		}
		public List<TerminalNode> COMMA() { return getTokens(Rybu4WSParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(Rybu4WSParser.COMMA, i);
		}
		public Instance_argumentsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_instance_arguments; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).enterInstance_arguments(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).exitInstance_arguments(this);
		}
	}

	public final Instance_argumentsContext instance_arguments() throws RecognitionException {
		Instance_argumentsContext _localctx = new Instance_argumentsContext(_ctx, getState());
		enterRule(_localctx, 6, RULE_instance_arguments);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(94);
			match(ID);
			setState(99);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(95);
				match(COMMA);
				setState(96);
				match(ID);
				}
				}
				setState(101);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Instance_statesContext extends ParserRuleContext {
		public TerminalNode LBRACE() { return getToken(Rybu4WSParser.LBRACE, 0); }
		public List<Instance_state_initContext> instance_state_init() {
			return getRuleContexts(Instance_state_initContext.class);
		}
		public Instance_state_initContext instance_state_init(int i) {
			return getRuleContext(Instance_state_initContext.class,i);
		}
		public TerminalNode RBRACE() { return getToken(Rybu4WSParser.RBRACE, 0); }
		public List<TerminalNode> COMMA() { return getTokens(Rybu4WSParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(Rybu4WSParser.COMMA, i);
		}
		public Instance_statesContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_instance_states; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).enterInstance_states(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).exitInstance_states(this);
		}
	}

	public final Instance_statesContext instance_states() throws RecognitionException {
		Instance_statesContext _localctx = new Instance_statesContext(_ctx, getState());
		enterRule(_localctx, 8, RULE_instance_states);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(102);
			match(LBRACE);
			setState(103);
			instance_state_init();
			setState(108);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(104);
				match(COMMA);
				setState(105);
				instance_state_init();
				}
				}
				setState(110);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(111);
			match(RBRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Instance_state_initContext extends ParserRuleContext {
		public TerminalNode ID() { return getToken(Rybu4WSParser.ID, 0); }
		public TerminalNode ASSIGNMENT() { return getToken(Rybu4WSParser.ASSIGNMENT, 0); }
		public TerminalNode NUMBER() { return getToken(Rybu4WSParser.NUMBER, 0); }
		public Enum_valueContext enum_value() {
			return getRuleContext(Enum_valueContext.class,0);
		}
		public Instance_state_initContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_instance_state_init; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).enterInstance_state_init(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).exitInstance_state_init(this);
		}
	}

	public final Instance_state_initContext instance_state_init() throws RecognitionException {
		Instance_state_initContext _localctx = new Instance_state_initContext(_ctx, getState());
		enterRule(_localctx, 10, RULE_instance_state_init);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(113);
			match(ID);
			setState(114);
			match(ASSIGNMENT);
			setState(117);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case NUMBER:
				{
				setState(115);
				match(NUMBER);
				}
				break;
			case COLON:
				{
				setState(116);
				enum_value();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Server_declarationContext extends ParserRuleContext {
		public TerminalNode SERVER() { return getToken(Rybu4WSParser.SERVER, 0); }
		public TerminalNode ID() { return getToken(Rybu4WSParser.ID, 0); }
		public TerminalNode LPAREN() { return getToken(Rybu4WSParser.LPAREN, 0); }
		public TerminalNode RPAREN() { return getToken(Rybu4WSParser.RPAREN, 0); }
		public TerminalNode LBRACE() { return getToken(Rybu4WSParser.LBRACE, 0); }
		public TerminalNode RBRACE() { return getToken(Rybu4WSParser.RBRACE, 0); }
		public Argument_with_type_listContext argument_with_type_list() {
			return getRuleContext(Argument_with_type_listContext.class,0);
		}
		public List<Variable_declarationContext> variable_declaration() {
			return getRuleContexts(Variable_declarationContext.class);
		}
		public Variable_declarationContext variable_declaration(int i) {
			return getRuleContext(Variable_declarationContext.class,i);
		}
		public List<Action_declarationContext> action_declaration() {
			return getRuleContexts(Action_declarationContext.class);
		}
		public Action_declarationContext action_declaration(int i) {
			return getRuleContext(Action_declarationContext.class,i);
		}
		public Server_declarationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_server_declaration; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).enterServer_declaration(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).exitServer_declaration(this);
		}
	}

	public final Server_declarationContext server_declaration() throws RecognitionException {
		Server_declarationContext _localctx = new Server_declarationContext(_ctx, getState());
		enterRule(_localctx, 12, RULE_server_declaration);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(119);
			match(SERVER);
			setState(120);
			match(ID);
			setState(121);
			match(LPAREN);
			setState(123);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==ID) {
				{
				setState(122);
				argument_with_type_list();
				}
			}

			setState(125);
			match(RPAREN);
			setState(126);
			match(LBRACE);
			setState(130);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==VAR) {
				{
				{
				setState(127);
				variable_declaration();
				}
				}
				setState(132);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(136);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==LBRACE) {
				{
				{
				setState(133);
				action_declaration();
				}
				}
				setState(138);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(139);
			match(RBRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Argument_with_type_listContext extends ParserRuleContext {
		public List<Argument_with_typeContext> argument_with_type() {
			return getRuleContexts(Argument_with_typeContext.class);
		}
		public Argument_with_typeContext argument_with_type(int i) {
			return getRuleContext(Argument_with_typeContext.class,i);
		}
		public List<TerminalNode> COMMA() { return getTokens(Rybu4WSParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(Rybu4WSParser.COMMA, i);
		}
		public Argument_with_type_listContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_argument_with_type_list; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).enterArgument_with_type_list(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).exitArgument_with_type_list(this);
		}
	}

	public final Argument_with_type_listContext argument_with_type_list() throws RecognitionException {
		Argument_with_type_listContext _localctx = new Argument_with_type_listContext(_ctx, getState());
		enterRule(_localctx, 14, RULE_argument_with_type_list);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(141);
			argument_with_type();
			setState(146);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(142);
				match(COMMA);
				setState(143);
				argument_with_type();
				}
				}
				setState(148);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Argument_with_typeContext extends ParserRuleContext {
		public List<TerminalNode> ID() { return getTokens(Rybu4WSParser.ID); }
		public TerminalNode ID(int i) {
			return getToken(Rybu4WSParser.ID, i);
		}
		public TerminalNode COLON() { return getToken(Rybu4WSParser.COLON, 0); }
		public Argument_with_typeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_argument_with_type; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).enterArgument_with_type(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).exitArgument_with_type(this);
		}
	}

	public final Argument_with_typeContext argument_with_type() throws RecognitionException {
		Argument_with_typeContext _localctx = new Argument_with_typeContext(_ctx, getState());
		enterRule(_localctx, 16, RULE_argument_with_type);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(149);
			match(ID);
			setState(150);
			match(COLON);
			setState(151);
			match(ID);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Variable_declarationContext extends ParserRuleContext {
		public TerminalNode VAR() { return getToken(Rybu4WSParser.VAR, 0); }
		public TerminalNode ID() { return getToken(Rybu4WSParser.ID, 0); }
		public TerminalNode COLON() { return getToken(Rybu4WSParser.COLON, 0); }
		public TerminalNode SEMICOLON() { return getToken(Rybu4WSParser.SEMICOLON, 0); }
		public Variable_type_integerContext variable_type_integer() {
			return getRuleContext(Variable_type_integerContext.class,0);
		}
		public Variable_type_enumContext variable_type_enum() {
			return getRuleContext(Variable_type_enumContext.class,0);
		}
		public Variable_declarationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_variable_declaration; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).enterVariable_declaration(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).exitVariable_declaration(this);
		}
	}

	public final Variable_declarationContext variable_declaration() throws RecognitionException {
		Variable_declarationContext _localctx = new Variable_declarationContext(_ctx, getState());
		enterRule(_localctx, 18, RULE_variable_declaration);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(153);
			match(VAR);
			setState(154);
			match(ID);
			setState(155);
			match(COLON);
			setState(158);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case NUMBER:
				{
				setState(156);
				variable_type_integer();
				}
				break;
			case LBRACE:
				{
				setState(157);
				variable_type_enum();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			setState(160);
			match(SEMICOLON);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Variable_type_integerContext extends ParserRuleContext {
		public List<TerminalNode> NUMBER() { return getTokens(Rybu4WSParser.NUMBER); }
		public TerminalNode NUMBER(int i) {
			return getToken(Rybu4WSParser.NUMBER, i);
		}
		public TerminalNode VAR_RANGE() { return getToken(Rybu4WSParser.VAR_RANGE, 0); }
		public Variable_type_integerContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_variable_type_integer; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).enterVariable_type_integer(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).exitVariable_type_integer(this);
		}
	}

	public final Variable_type_integerContext variable_type_integer() throws RecognitionException {
		Variable_type_integerContext _localctx = new Variable_type_integerContext(_ctx, getState());
		enterRule(_localctx, 20, RULE_variable_type_integer);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(162);
			match(NUMBER);
			setState(163);
			match(VAR_RANGE);
			setState(164);
			match(NUMBER);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Variable_type_enumContext extends ParserRuleContext {
		public TerminalNode LBRACE() { return getToken(Rybu4WSParser.LBRACE, 0); }
		public List<TerminalNode> ID() { return getTokens(Rybu4WSParser.ID); }
		public TerminalNode ID(int i) {
			return getToken(Rybu4WSParser.ID, i);
		}
		public TerminalNode RBRACE() { return getToken(Rybu4WSParser.RBRACE, 0); }
		public List<TerminalNode> COMMA() { return getTokens(Rybu4WSParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(Rybu4WSParser.COMMA, i);
		}
		public Variable_type_enumContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_variable_type_enum; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).enterVariable_type_enum(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).exitVariable_type_enum(this);
		}
	}

	public final Variable_type_enumContext variable_type_enum() throws RecognitionException {
		Variable_type_enumContext _localctx = new Variable_type_enumContext(_ctx, getState());
		enterRule(_localctx, 22, RULE_variable_type_enum);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(166);
			match(LBRACE);
			setState(167);
			match(ID);
			setState(172);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(168);
				match(COMMA);
				setState(169);
				match(ID);
				}
				}
				setState(174);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(175);
			match(RBRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Action_declarationContext extends ParserRuleContext {
		public List<TerminalNode> LBRACE() { return getTokens(Rybu4WSParser.LBRACE); }
		public TerminalNode LBRACE(int i) {
			return getToken(Rybu4WSParser.LBRACE, i);
		}
		public TerminalNode ID() { return getToken(Rybu4WSParser.ID, 0); }
		public List<TerminalNode> RBRACE() { return getTokens(Rybu4WSParser.RBRACE); }
		public TerminalNode RBRACE(int i) {
			return getToken(Rybu4WSParser.RBRACE, i);
		}
		public TerminalNode ACTION_ARROW() { return getToken(Rybu4WSParser.ACTION_ARROW, 0); }
		public Action_conditionContext action_condition() {
			return getRuleContext(Action_conditionContext.class,0);
		}
		public List<StatementContext> statement() {
			return getRuleContexts(StatementContext.class);
		}
		public StatementContext statement(int i) {
			return getRuleContext(StatementContext.class,i);
		}
		public Action_declarationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_action_declaration; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).enterAction_declaration(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).exitAction_declaration(this);
		}
	}

	public final Action_declarationContext action_declaration() throws RecognitionException {
		Action_declarationContext _localctx = new Action_declarationContext(_ctx, getState());
		enterRule(_localctx, 24, RULE_action_declaration);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(177);
			match(LBRACE);
			setState(178);
			match(ID);
			setState(180);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==ACTION_CONDITION) {
				{
				setState(179);
				action_condition();
				}
			}

			setState(182);
			match(RBRACE);
			setState(183);
			match(ACTION_ARROW);
			setState(184);
			match(LBRACE);
			setState(188);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << MATCH) | (1L << RETURN) | (1L << ID))) != 0)) {
				{
				{
				setState(185);
				statement();
				}
				}
				setState(190);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(191);
			match(RBRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Action_conditionContext extends ParserRuleContext {
		public TerminalNode ACTION_CONDITION() { return getToken(Rybu4WSParser.ACTION_CONDITION, 0); }
		public List<ConditionContext> condition() {
			return getRuleContexts(ConditionContext.class);
		}
		public ConditionContext condition(int i) {
			return getRuleContext(ConditionContext.class,i);
		}
		public List<Condition_logic_operatorContext> condition_logic_operator() {
			return getRuleContexts(Condition_logic_operatorContext.class);
		}
		public Condition_logic_operatorContext condition_logic_operator(int i) {
			return getRuleContext(Condition_logic_operatorContext.class,i);
		}
		public Action_conditionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_action_condition; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).enterAction_condition(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).exitAction_condition(this);
		}
	}

	public final Action_conditionContext action_condition() throws RecognitionException {
		Action_conditionContext _localctx = new Action_conditionContext(_ctx, getState());
		enterRule(_localctx, 26, RULE_action_condition);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(193);
			match(ACTION_CONDITION);
			setState(194);
			condition();
			setState(200);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==CONDITION_AND || _la==CONDITION_OR) {
				{
				{
				setState(195);
				condition_logic_operator();
				setState(196);
				condition();
				}
				}
				setState(202);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ConditionContext extends ParserRuleContext {
		public TerminalNode ID() { return getToken(Rybu4WSParser.ID, 0); }
		public Condition_comparison_operatorContext condition_comparison_operator() {
			return getRuleContext(Condition_comparison_operatorContext.class,0);
		}
		public TerminalNode NUMBER() { return getToken(Rybu4WSParser.NUMBER, 0); }
		public Enum_valueContext enum_value() {
			return getRuleContext(Enum_valueContext.class,0);
		}
		public ConditionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_condition; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).enterCondition(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).exitCondition(this);
		}
	}

	public final ConditionContext condition() throws RecognitionException {
		ConditionContext _localctx = new ConditionContext(_ctx, getState());
		enterRule(_localctx, 28, RULE_condition);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(203);
			match(ID);
			setState(204);
			condition_comparison_operator();
			setState(207);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case NUMBER:
				{
				setState(205);
				match(NUMBER);
				}
				break;
			case COLON:
				{
				setState(206);
				enum_value();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Condition_logic_operatorContext extends ParserRuleContext {
		public TerminalNode CONDITION_AND() { return getToken(Rybu4WSParser.CONDITION_AND, 0); }
		public TerminalNode CONDITION_OR() { return getToken(Rybu4WSParser.CONDITION_OR, 0); }
		public Condition_logic_operatorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_condition_logic_operator; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).enterCondition_logic_operator(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).exitCondition_logic_operator(this);
		}
	}

	public final Condition_logic_operatorContext condition_logic_operator() throws RecognitionException {
		Condition_logic_operatorContext _localctx = new Condition_logic_operatorContext(_ctx, getState());
		enterRule(_localctx, 30, RULE_condition_logic_operator);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(209);
			_la = _input.LA(1);
			if ( !(_la==CONDITION_AND || _la==CONDITION_OR) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Condition_comparison_operatorContext extends ParserRuleContext {
		public TerminalNode CONDITION_EQUAL() { return getToken(Rybu4WSParser.CONDITION_EQUAL, 0); }
		public TerminalNode CONDITION_NOT_EQUAL() { return getToken(Rybu4WSParser.CONDITION_NOT_EQUAL, 0); }
		public TerminalNode CONDITION_GREATER_THAN() { return getToken(Rybu4WSParser.CONDITION_GREATER_THAN, 0); }
		public TerminalNode CONDITION_LESS_THAN() { return getToken(Rybu4WSParser.CONDITION_LESS_THAN, 0); }
		public TerminalNode CONDITION_GREATER_OR_EQUAL_THAN() { return getToken(Rybu4WSParser.CONDITION_GREATER_OR_EQUAL_THAN, 0); }
		public TerminalNode CONDITION_LESS_OR_EQUAL_THAN() { return getToken(Rybu4WSParser.CONDITION_LESS_OR_EQUAL_THAN, 0); }
		public Condition_comparison_operatorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_condition_comparison_operator; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).enterCondition_comparison_operator(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).exitCondition_comparison_operator(this);
		}
	}

	public final Condition_comparison_operatorContext condition_comparison_operator() throws RecognitionException {
		Condition_comparison_operatorContext _localctx = new Condition_comparison_operatorContext(_ctx, getState());
		enterRule(_localctx, 32, RULE_condition_comparison_operator);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(211);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << CONDITION_EQUAL) | (1L << CONDITION_NOT_EQUAL) | (1L << CONDITION_GREATER_THAN) | (1L << CONDITION_LESS_THAN) | (1L << CONDITION_GREATER_OR_EQUAL_THAN) | (1L << CONDITION_LESS_OR_EQUAL_THAN))) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class StatementContext extends ParserRuleContext {
		public Statement_callContext statement_call() {
			return getRuleContext(Statement_callContext.class,0);
		}
		public Statement_matchContext statement_match() {
			return getRuleContext(Statement_matchContext.class,0);
		}
		public Statement_state_mutationContext statement_state_mutation() {
			return getRuleContext(Statement_state_mutationContext.class,0);
		}
		public Statement_returnContext statement_return() {
			return getRuleContext(Statement_returnContext.class,0);
		}
		public StatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_statement; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).enterStatement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).exitStatement(this);
		}
	}

	public final StatementContext statement() throws RecognitionException {
		StatementContext _localctx = new StatementContext(_ctx, getState());
		enterRule(_localctx, 34, RULE_statement);
		try {
			setState(217);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,19,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(213);
				statement_call();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(214);
				statement_match();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(215);
				statement_state_mutation();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(216);
				statement_return();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Statement_callContext extends ParserRuleContext {
		public List<TerminalNode> ID() { return getTokens(Rybu4WSParser.ID); }
		public TerminalNode ID(int i) {
			return getToken(Rybu4WSParser.ID, i);
		}
		public TerminalNode DOT() { return getToken(Rybu4WSParser.DOT, 0); }
		public TerminalNode LPAREN() { return getToken(Rybu4WSParser.LPAREN, 0); }
		public TerminalNode RPAREN() { return getToken(Rybu4WSParser.RPAREN, 0); }
		public TerminalNode SEMICOLON() { return getToken(Rybu4WSParser.SEMICOLON, 0); }
		public Statement_callContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_statement_call; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).enterStatement_call(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).exitStatement_call(this);
		}
	}

	public final Statement_callContext statement_call() throws RecognitionException {
		Statement_callContext _localctx = new Statement_callContext(_ctx, getState());
		enterRule(_localctx, 36, RULE_statement_call);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(219);
			match(ID);
			setState(220);
			match(DOT);
			setState(221);
			match(ID);
			setState(222);
			match(LPAREN);
			setState(223);
			match(RPAREN);
			setState(224);
			match(SEMICOLON);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Statement_matchContext extends ParserRuleContext {
		public TerminalNode MATCH() { return getToken(Rybu4WSParser.MATCH, 0); }
		public List<TerminalNode> ID() { return getTokens(Rybu4WSParser.ID); }
		public TerminalNode ID(int i) {
			return getToken(Rybu4WSParser.ID, i);
		}
		public TerminalNode DOT() { return getToken(Rybu4WSParser.DOT, 0); }
		public TerminalNode LPAREN() { return getToken(Rybu4WSParser.LPAREN, 0); }
		public TerminalNode RPAREN() { return getToken(Rybu4WSParser.RPAREN, 0); }
		public TerminalNode LBRACE() { return getToken(Rybu4WSParser.LBRACE, 0); }
		public TerminalNode RBRACE() { return getToken(Rybu4WSParser.RBRACE, 0); }
		public List<Statement_match_optionContext> statement_match_option() {
			return getRuleContexts(Statement_match_optionContext.class);
		}
		public Statement_match_optionContext statement_match_option(int i) {
			return getRuleContext(Statement_match_optionContext.class,i);
		}
		public Statement_matchContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_statement_match; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).enterStatement_match(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).exitStatement_match(this);
		}
	}

	public final Statement_matchContext statement_match() throws RecognitionException {
		Statement_matchContext _localctx = new Statement_matchContext(_ctx, getState());
		enterRule(_localctx, 38, RULE_statement_match);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(226);
			match(MATCH);
			setState(227);
			match(ID);
			setState(228);
			match(DOT);
			setState(229);
			match(ID);
			setState(230);
			match(LPAREN);
			setState(231);
			match(RPAREN);
			setState(232);
			match(LBRACE);
			setState(236);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COLON) {
				{
				{
				setState(233);
				statement_match_option();
				}
				}
				setState(238);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(239);
			match(RBRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Statement_match_optionContext extends ParserRuleContext {
		public Enum_valueContext enum_value() {
			return getRuleContext(Enum_valueContext.class,0);
		}
		public TerminalNode ACTION_ARROW() { return getToken(Rybu4WSParser.ACTION_ARROW, 0); }
		public TerminalNode LBRACE() { return getToken(Rybu4WSParser.LBRACE, 0); }
		public TerminalNode RBRACE() { return getToken(Rybu4WSParser.RBRACE, 0); }
		public TerminalNode MATCH_SKIP() { return getToken(Rybu4WSParser.MATCH_SKIP, 0); }
		public TerminalNode SEMICOLON() { return getToken(Rybu4WSParser.SEMICOLON, 0); }
		public List<StatementContext> statement() {
			return getRuleContexts(StatementContext.class);
		}
		public StatementContext statement(int i) {
			return getRuleContext(StatementContext.class,i);
		}
		public Statement_match_optionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_statement_match_option; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).enterStatement_match_option(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).exitStatement_match_option(this);
		}
	}

	public final Statement_match_optionContext statement_match_option() throws RecognitionException {
		Statement_match_optionContext _localctx = new Statement_match_optionContext(_ctx, getState());
		enterRule(_localctx, 40, RULE_statement_match_option);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(241);
			enum_value();
			setState(242);
			match(ACTION_ARROW);
			setState(243);
			match(LBRACE);
			setState(252);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case RBRACE:
			case MATCH:
			case RETURN:
			case ID:
				{
				setState(247);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << MATCH) | (1L << RETURN) | (1L << ID))) != 0)) {
					{
					{
					setState(244);
					statement();
					}
					}
					setState(249);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				}
				break;
			case MATCH_SKIP:
				{
				{
				setState(250);
				match(MATCH_SKIP);
				setState(251);
				match(SEMICOLON);
				}
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			setState(254);
			match(RBRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Statement_state_mutationContext extends ParserRuleContext {
		public TerminalNode ID() { return getToken(Rybu4WSParser.ID, 0); }
		public Statement_state_mutation_operatorContext statement_state_mutation_operator() {
			return getRuleContext(Statement_state_mutation_operatorContext.class,0);
		}
		public TerminalNode SEMICOLON() { return getToken(Rybu4WSParser.SEMICOLON, 0); }
		public TerminalNode NUMBER() { return getToken(Rybu4WSParser.NUMBER, 0); }
		public Enum_valueContext enum_value() {
			return getRuleContext(Enum_valueContext.class,0);
		}
		public Statement_state_mutationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_statement_state_mutation; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).enterStatement_state_mutation(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).exitStatement_state_mutation(this);
		}
	}

	public final Statement_state_mutationContext statement_state_mutation() throws RecognitionException {
		Statement_state_mutationContext _localctx = new Statement_state_mutationContext(_ctx, getState());
		enterRule(_localctx, 42, RULE_statement_state_mutation);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(256);
			match(ID);
			setState(257);
			statement_state_mutation_operator();
			setState(260);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case NUMBER:
				{
				setState(258);
				match(NUMBER);
				}
				break;
			case COLON:
				{
				setState(259);
				enum_value();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			setState(262);
			match(SEMICOLON);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Statement_state_mutation_operatorContext extends ParserRuleContext {
		public TerminalNode ASSIGNMENT() { return getToken(Rybu4WSParser.ASSIGNMENT, 0); }
		public TerminalNode OPERATOR_INCREMENT() { return getToken(Rybu4WSParser.OPERATOR_INCREMENT, 0); }
		public TerminalNode OPERATOR_DECREMENT() { return getToken(Rybu4WSParser.OPERATOR_DECREMENT, 0); }
		public Statement_state_mutation_operatorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_statement_state_mutation_operator; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).enterStatement_state_mutation_operator(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).exitStatement_state_mutation_operator(this);
		}
	}

	public final Statement_state_mutation_operatorContext statement_state_mutation_operator() throws RecognitionException {
		Statement_state_mutation_operatorContext _localctx = new Statement_state_mutation_operatorContext(_ctx, getState());
		enterRule(_localctx, 44, RULE_statement_state_mutation_operator);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(264);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << ASSIGNMENT) | (1L << OPERATOR_INCREMENT) | (1L << OPERATOR_DECREMENT))) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Statement_returnContext extends ParserRuleContext {
		public TerminalNode RETURN() { return getToken(Rybu4WSParser.RETURN, 0); }
		public Enum_valueContext enum_value() {
			return getRuleContext(Enum_valueContext.class,0);
		}
		public TerminalNode SEMICOLON() { return getToken(Rybu4WSParser.SEMICOLON, 0); }
		public Statement_returnContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_statement_return; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).enterStatement_return(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).exitStatement_return(this);
		}
	}

	public final Statement_returnContext statement_return() throws RecognitionException {
		Statement_returnContext _localctx = new Statement_returnContext(_ctx, getState());
		enterRule(_localctx, 46, RULE_statement_return);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(266);
			match(RETURN);
			setState(267);
			enum_value();
			setState(268);
			match(SEMICOLON);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Enum_valueContext extends ParserRuleContext {
		public TerminalNode COLON() { return getToken(Rybu4WSParser.COLON, 0); }
		public TerminalNode ID() { return getToken(Rybu4WSParser.ID, 0); }
		public Enum_valueContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_enum_value; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).enterEnum_value(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof Rybu4WSListener ) ((Rybu4WSListener)listener).exitEnum_value(this);
		}
	}

	public final Enum_valueContext enum_value() throws RecognitionException {
		Enum_valueContext _localctx = new Enum_valueContext(_ctx, getState());
		enterRule(_localctx, 48, RULE_enum_value);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(270);
			match(COLON);
			setState(271);
			match(ID);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static final String _serializedATN =
		"\3\u608b\ua72a\u8133\ub9ed\u417c\u3be7\u7786\u5964\3!\u0114\4\2\t\2\4"+
		"\3\t\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b\t\b\4\t\t\t\4\n\t\n\4\13\t"+
		"\13\4\f\t\f\4\r\t\r\4\16\t\16\4\17\t\17\4\20\t\20\4\21\t\21\4\22\t\22"+
		"\4\23\t\23\4\24\t\24\4\25\t\25\4\26\t\26\4\27\t\27\4\30\t\30\4\31\t\31"+
		"\4\32\t\32\3\2\7\2\66\n\2\f\2\16\29\13\2\3\2\7\2<\n\2\f\2\16\2?\13\2\3"+
		"\2\7\2B\n\2\f\2\16\2E\13\2\3\2\3\2\3\3\3\3\3\3\7\3L\n\3\f\3\16\3O\13\3"+
		"\3\3\3\3\3\4\3\4\3\4\3\4\3\4\3\4\5\4Y\n\4\3\4\3\4\5\4]\n\4\3\4\3\4\3\5"+
		"\3\5\3\5\7\5d\n\5\f\5\16\5g\13\5\3\6\3\6\3\6\3\6\7\6m\n\6\f\6\16\6p\13"+
		"\6\3\6\3\6\3\7\3\7\3\7\3\7\5\7x\n\7\3\b\3\b\3\b\3\b\5\b~\n\b\3\b\3\b\3"+
		"\b\7\b\u0083\n\b\f\b\16\b\u0086\13\b\3\b\7\b\u0089\n\b\f\b\16\b\u008c"+
		"\13\b\3\b\3\b\3\t\3\t\3\t\7\t\u0093\n\t\f\t\16\t\u0096\13\t\3\n\3\n\3"+
		"\n\3\n\3\13\3\13\3\13\3\13\3\13\5\13\u00a1\n\13\3\13\3\13\3\f\3\f\3\f"+
		"\3\f\3\r\3\r\3\r\3\r\7\r\u00ad\n\r\f\r\16\r\u00b0\13\r\3\r\3\r\3\16\3"+
		"\16\3\16\5\16\u00b7\n\16\3\16\3\16\3\16\3\16\7\16\u00bd\n\16\f\16\16\16"+
		"\u00c0\13\16\3\16\3\16\3\17\3\17\3\17\3\17\3\17\7\17\u00c9\n\17\f\17\16"+
		"\17\u00cc\13\17\3\20\3\20\3\20\3\20\5\20\u00d2\n\20\3\21\3\21\3\22\3\22"+
		"\3\23\3\23\3\23\3\23\5\23\u00dc\n\23\3\24\3\24\3\24\3\24\3\24\3\24\3\24"+
		"\3\25\3\25\3\25\3\25\3\25\3\25\3\25\3\25\7\25\u00ed\n\25\f\25\16\25\u00f0"+
		"\13\25\3\25\3\25\3\26\3\26\3\26\3\26\7\26\u00f8\n\26\f\26\16\26\u00fb"+
		"\13\26\3\26\3\26\5\26\u00ff\n\26\3\26\3\26\3\27\3\27\3\27\3\27\5\27\u0107"+
		"\n\27\3\27\3\27\3\30\3\30\3\31\3\31\3\31\3\31\3\32\3\32\3\32\3\32\2\2"+
		"\33\2\4\6\b\n\f\16\20\22\24\26\30\32\34\36 \"$&(*,.\60\62\2\5\3\2\30\31"+
		"\3\2\22\27\3\2\17\21\2\u0114\2\67\3\2\2\2\4H\3\2\2\2\6R\3\2\2\2\b`\3\2"+
		"\2\2\nh\3\2\2\2\fs\3\2\2\2\16y\3\2\2\2\20\u008f\3\2\2\2\22\u0097\3\2\2"+
		"\2\24\u009b\3\2\2\2\26\u00a4\3\2\2\2\30\u00a8\3\2\2\2\32\u00b3\3\2\2\2"+
		"\34\u00c3\3\2\2\2\36\u00cd\3\2\2\2 \u00d3\3\2\2\2\"\u00d5\3\2\2\2$\u00db"+
		"\3\2\2\2&\u00dd\3\2\2\2(\u00e4\3\2\2\2*\u00f3\3\2\2\2,\u0102\3\2\2\2."+
		"\u010a\3\2\2\2\60\u010c\3\2\2\2\62\u0110\3\2\2\2\64\66\5\16\b\2\65\64"+
		"\3\2\2\2\669\3\2\2\2\67\65\3\2\2\2\678\3\2\2\28=\3\2\2\29\67\3\2\2\2:"+
		"<\5\6\4\2;:\3\2\2\2<?\3\2\2\2=;\3\2\2\2=>\3\2\2\2>C\3\2\2\2?=\3\2\2\2"+
		"@B\5\4\3\2A@\3\2\2\2BE\3\2\2\2CA\3\2\2\2CD\3\2\2\2DF\3\2\2\2EC\3\2\2\2"+
		"FG\7\2\2\3G\3\3\2\2\2HI\7\36\2\2IM\7\4\2\2JL\5$\23\2KJ\3\2\2\2LO\3\2\2"+
		"\2MK\3\2\2\2MN\3\2\2\2NP\3\2\2\2OM\3\2\2\2PQ\7\5\2\2Q\5\3\2\2\2RS\7\13"+
		"\2\2ST\7 \2\2TU\7\17\2\2UV\7 \2\2VX\7\6\2\2WY\5\b\5\2XW\3\2\2\2XY\3\2"+
		"\2\2YZ\3\2\2\2Z\\\7\7\2\2[]\5\n\6\2\\[\3\2\2\2\\]\3\2\2\2]^\3\2\2\2^_"+
		"\7\n\2\2_\7\3\2\2\2`e\7 \2\2ab\7\t\2\2bd\7 \2\2ca\3\2\2\2dg\3\2\2\2ec"+
		"\3\2\2\2ef\3\2\2\2f\t\3\2\2\2ge\3\2\2\2hi\7\4\2\2in\5\f\7\2jk\7\t\2\2"+
		"km\5\f\7\2lj\3\2\2\2mp\3\2\2\2nl\3\2\2\2no\3\2\2\2oq\3\2\2\2pn\3\2\2\2"+
		"qr\7\5\2\2r\13\3\2\2\2st\7 \2\2tw\7\17\2\2ux\7\37\2\2vx\5\62\32\2wu\3"+
		"\2\2\2wv\3\2\2\2x\r\3\2\2\2yz\7\35\2\2z{\7 \2\2{}\7\6\2\2|~\5\20\t\2}"+
		"|\3\2\2\2}~\3\2\2\2~\177\3\2\2\2\177\u0080\7\7\2\2\u0080\u0084\7\4\2\2"+
		"\u0081\u0083\5\24\13\2\u0082\u0081\3\2\2\2\u0083\u0086\3\2\2\2\u0084\u0082"+
		"\3\2\2\2\u0084\u0085\3\2\2\2\u0085\u008a\3\2\2\2\u0086\u0084\3\2\2\2\u0087"+
		"\u0089\5\32\16\2\u0088\u0087\3\2\2\2\u0089\u008c\3\2\2\2\u008a\u0088\3"+
		"\2\2\2\u008a\u008b\3\2\2\2\u008b\u008d\3\2\2\2\u008c\u008a\3\2\2\2\u008d"+
		"\u008e\7\5\2\2\u008e\17\3\2\2\2\u008f\u0094\5\22\n\2\u0090\u0091\7\t\2"+
		"\2\u0091\u0093\5\22\n\2\u0092\u0090\3\2\2\2\u0093\u0096\3\2\2\2\u0094"+
		"\u0092\3\2\2\2\u0094\u0095\3\2\2\2\u0095\21\3\2\2\2\u0096\u0094\3\2\2"+
		"\2\u0097\u0098\7 \2\2\u0098\u0099\7\b\2\2\u0099\u009a\7 \2\2\u009a\23"+
		"\3\2\2\2\u009b\u009c\7\13\2\2\u009c\u009d\7 \2\2\u009d\u00a0\7\b\2\2\u009e"+
		"\u00a1\5\26\f\2\u009f\u00a1\5\30\r\2\u00a0\u009e\3\2\2\2\u00a0\u009f\3"+
		"\2\2\2\u00a1\u00a2\3\2\2\2\u00a2\u00a3\7\n\2\2\u00a3\25\3\2\2\2\u00a4"+
		"\u00a5\7\37\2\2\u00a5\u00a6\7\16\2\2\u00a6\u00a7\7\37\2\2\u00a7\27\3\2"+
		"\2\2\u00a8\u00a9\7\4\2\2\u00a9\u00ae\7 \2\2\u00aa\u00ab\7\t\2\2\u00ab"+
		"\u00ad\7 \2\2\u00ac\u00aa\3\2\2\2\u00ad\u00b0\3\2\2\2\u00ae\u00ac\3\2"+
		"\2\2\u00ae\u00af\3\2\2\2\u00af\u00b1\3\2\2\2\u00b0\u00ae\3\2\2\2\u00b1"+
		"\u00b2\7\5\2\2\u00b2\31\3\2\2\2\u00b3\u00b4\7\4\2\2\u00b4\u00b6\7 \2\2"+
		"\u00b5\u00b7\5\34\17\2\u00b6\u00b5\3\2\2\2\u00b6\u00b7\3\2\2\2\u00b7\u00b8"+
		"\3\2\2\2\u00b8\u00b9\7\5\2\2\u00b9\u00ba\7\r\2\2\u00ba\u00be\7\4\2\2\u00bb"+
		"\u00bd\5$\23\2\u00bc\u00bb\3\2\2\2\u00bd\u00c0\3\2\2\2\u00be\u00bc\3\2"+
		"\2\2\u00be\u00bf\3\2\2\2\u00bf\u00c1\3\2\2\2\u00c0\u00be\3\2\2\2\u00c1"+
		"\u00c2\7\5\2\2\u00c2\33\3\2\2\2\u00c3\u00c4\7\f\2\2\u00c4\u00ca\5\36\20"+
		"\2\u00c5\u00c6\5 \21\2\u00c6\u00c7\5\36\20\2\u00c7\u00c9\3\2\2\2\u00c8"+
		"\u00c5\3\2\2\2\u00c9\u00cc\3\2\2\2\u00ca\u00c8\3\2\2\2\u00ca\u00cb\3\2"+
		"\2\2\u00cb\35\3\2\2\2\u00cc\u00ca\3\2\2\2\u00cd\u00ce\7 \2\2\u00ce\u00d1"+
		"\5\"\22\2\u00cf\u00d2\7\37\2\2\u00d0\u00d2\5\62\32\2\u00d1\u00cf\3\2\2"+
		"\2\u00d1\u00d0\3\2\2\2\u00d2\37\3\2\2\2\u00d3\u00d4\t\2\2\2\u00d4!\3\2"+
		"\2\2\u00d5\u00d6\t\3\2\2\u00d6#\3\2\2\2\u00d7\u00dc\5&\24\2\u00d8\u00dc"+
		"\5(\25\2\u00d9\u00dc\5,\27\2\u00da\u00dc\5\60\31\2\u00db\u00d7\3\2\2\2"+
		"\u00db\u00d8\3\2\2\2\u00db\u00d9\3\2\2\2\u00db\u00da\3\2\2\2\u00dc%\3"+
		"\2\2\2\u00dd\u00de\7 \2\2\u00de\u00df\7\3\2\2\u00df\u00e0\7 \2\2\u00e0"+
		"\u00e1\7\6\2\2\u00e1\u00e2\7\7\2\2\u00e2\u00e3\7\n\2\2\u00e3\'\3\2\2\2"+
		"\u00e4\u00e5\7\33\2\2\u00e5\u00e6\7 \2\2\u00e6\u00e7\7\3\2\2\u00e7\u00e8"+
		"\7 \2\2\u00e8\u00e9\7\6\2\2\u00e9\u00ea\7\7\2\2\u00ea\u00ee\7\4\2\2\u00eb"+
		"\u00ed\5*\26\2\u00ec\u00eb\3\2\2\2\u00ed\u00f0\3\2\2\2\u00ee\u00ec\3\2"+
		"\2\2\u00ee\u00ef\3\2\2\2\u00ef\u00f1\3\2\2\2\u00f0\u00ee\3\2\2\2\u00f1"+
		"\u00f2\7\5\2\2\u00f2)\3\2\2\2\u00f3\u00f4\5\62\32\2\u00f4\u00f5\7\r\2"+
		"\2\u00f5\u00fe\7\4\2\2\u00f6\u00f8\5$\23\2\u00f7\u00f6\3\2\2\2\u00f8\u00fb"+
		"\3\2\2\2\u00f9\u00f7\3\2\2\2\u00f9\u00fa\3\2\2\2\u00fa\u00ff\3\2\2\2\u00fb"+
		"\u00f9\3\2\2\2\u00fc\u00fd\7\32\2\2\u00fd\u00ff\7\n\2\2\u00fe\u00f9\3"+
		"\2\2\2\u00fe\u00fc\3\2\2\2\u00ff\u0100\3\2\2\2\u0100\u0101\7\5\2\2\u0101"+
		"+\3\2\2\2\u0102\u0103\7 \2\2\u0103\u0106\5.\30\2\u0104\u0107\7\37\2\2"+
		"\u0105\u0107\5\62\32\2\u0106\u0104\3\2\2\2\u0106\u0105\3\2\2\2\u0107\u0108"+
		"\3\2\2\2\u0108\u0109\7\n\2\2\u0109-\3\2\2\2\u010a\u010b\t\4\2\2\u010b"+
		"/\3\2\2\2\u010c\u010d\7\34\2\2\u010d\u010e\5\62\32\2\u010e\u010f\7\n\2"+
		"\2\u010f\61\3\2\2\2\u0110\u0111\7\b\2\2\u0111\u0112\7 \2\2\u0112\63\3"+
		"\2\2\2\32\67=CMX\\enw}\u0084\u008a\u0094\u00a0\u00ae\u00b6\u00be\u00ca"+
		"\u00d1\u00db\u00ee\u00f9\u00fe\u0106";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}