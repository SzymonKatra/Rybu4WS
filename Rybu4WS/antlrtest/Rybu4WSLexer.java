// Generated from ../Rybu4WS.g4 by ANTLR 4.9.2
import org.antlr.v4.runtime.Lexer;
import org.antlr.v4.runtime.CharStream;
import org.antlr.v4.runtime.Token;
import org.antlr.v4.runtime.TokenStream;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.misc.*;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast"})
public class Rybu4WSLexer extends Lexer {
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
	public static String[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static String[] modeNames = {
		"DEFAULT_MODE"
	};

	private static String[] makeRuleNames() {
		return new String[] {
			"DOT", "LBRACE", "RBRACE", "LPAREN", "RPAREN", "COLON", "COMMA", "SEMICOLON", 
			"VAR", "ACTION_CONDITION", "ACTION_ARROW", "VAR_RANGE", "ASSIGNMENT", 
			"OPERATOR_INCREMENT", "OPERATOR_DECREMENT", "CONDITION_EQUAL", "CONDITION_NOT_EQUAL", 
			"CONDITION_GREATER_THAN", "CONDITION_LESS_THAN", "CONDITION_GREATER_OR_EQUAL_THAN", 
			"CONDITION_LESS_OR_EQUAL_THAN", "CONDITION_AND", "CONDITION_OR", "MATCH_SKIP", 
			"MATCH", "RETURN", "SERVER", "PROCESS", "NUMBER", "ID", "WHITESPACE"
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


	public Rybu4WSLexer(CharStream input) {
		super(input);
		_interp = new LexerATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	@Override
	public String getGrammarFileName() { return "Rybu4WS.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public String[] getChannelNames() { return channelNames; }

	@Override
	public String[] getModeNames() { return modeNames; }

	@Override
	public ATN getATN() { return _ATN; }

	public static final String _serializedATN =
		"\3\u608b\ua72a\u8133\ub9ed\u417c\u3be7\u7786\u5964\2!\u00ae\b\1\4\2\t"+
		"\2\4\3\t\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b\t\b\4\t\t\t\4\n\t\n\4\13"+
		"\t\13\4\f\t\f\4\r\t\r\4\16\t\16\4\17\t\17\4\20\t\20\4\21\t\21\4\22\t\22"+
		"\4\23\t\23\4\24\t\24\4\25\t\25\4\26\t\26\4\27\t\27\4\30\t\30\4\31\t\31"+
		"\4\32\t\32\4\33\t\33\4\34\t\34\4\35\t\35\4\36\t\36\4\37\t\37\4 \t \3\2"+
		"\3\2\3\3\3\3\3\4\3\4\3\5\3\5\3\6\3\6\3\7\3\7\3\b\3\b\3\t\3\t\3\n\3\n\3"+
		"\n\3\n\3\13\3\13\3\f\3\f\3\f\3\r\3\r\3\r\3\16\3\16\3\17\3\17\3\17\3\20"+
		"\3\20\3\20\3\21\3\21\3\21\3\22\3\22\3\22\3\23\3\23\3\24\3\24\3\25\3\25"+
		"\3\25\3\26\3\26\3\26\3\27\3\27\3\27\3\30\3\30\3\30\3\31\3\31\3\31\3\31"+
		"\3\31\3\32\3\32\3\32\3\32\3\32\3\32\3\33\3\33\3\33\3\33\3\33\3\33\3\33"+
		"\3\34\3\34\3\34\3\34\3\34\3\34\3\34\3\35\3\35\3\35\3\35\3\35\3\35\3\35"+
		"\3\35\3\36\6\36\u009e\n\36\r\36\16\36\u009f\3\37\3\37\6\37\u00a4\n\37"+
		"\r\37\16\37\u00a5\3 \6 \u00a9\n \r \16 \u00aa\3 \3 \2\2!\3\3\5\4\7\5\t"+
		"\6\13\7\r\b\17\t\21\n\23\13\25\f\27\r\31\16\33\17\35\20\37\21!\22#\23"+
		"%\24\'\25)\26+\27-\30/\31\61\32\63\33\65\34\67\359\36;\37= ?!\3\2\6\3"+
		"\2\62;\4\2C\\c|\7\2//\62;C\\aac|\5\2\13\f\17\17\"\"\2\u00b0\2\3\3\2\2"+
		"\2\2\5\3\2\2\2\2\7\3\2\2\2\2\t\3\2\2\2\2\13\3\2\2\2\2\r\3\2\2\2\2\17\3"+
		"\2\2\2\2\21\3\2\2\2\2\23\3\2\2\2\2\25\3\2\2\2\2\27\3\2\2\2\2\31\3\2\2"+
		"\2\2\33\3\2\2\2\2\35\3\2\2\2\2\37\3\2\2\2\2!\3\2\2\2\2#\3\2\2\2\2%\3\2"+
		"\2\2\2\'\3\2\2\2\2)\3\2\2\2\2+\3\2\2\2\2-\3\2\2\2\2/\3\2\2\2\2\61\3\2"+
		"\2\2\2\63\3\2\2\2\2\65\3\2\2\2\2\67\3\2\2\2\29\3\2\2\2\2;\3\2\2\2\2=\3"+
		"\2\2\2\2?\3\2\2\2\3A\3\2\2\2\5C\3\2\2\2\7E\3\2\2\2\tG\3\2\2\2\13I\3\2"+
		"\2\2\rK\3\2\2\2\17M\3\2\2\2\21O\3\2\2\2\23Q\3\2\2\2\25U\3\2\2\2\27W\3"+
		"\2\2\2\31Z\3\2\2\2\33]\3\2\2\2\35_\3\2\2\2\37b\3\2\2\2!e\3\2\2\2#h\3\2"+
		"\2\2%k\3\2\2\2\'m\3\2\2\2)o\3\2\2\2+r\3\2\2\2-u\3\2\2\2/x\3\2\2\2\61{"+
		"\3\2\2\2\63\u0080\3\2\2\2\65\u0086\3\2\2\2\67\u008d\3\2\2\29\u0094\3\2"+
		"\2\2;\u009d\3\2\2\2=\u00a1\3\2\2\2?\u00a8\3\2\2\2AB\7\60\2\2B\4\3\2\2"+
		"\2CD\7}\2\2D\6\3\2\2\2EF\7\177\2\2F\b\3\2\2\2GH\7*\2\2H\n\3\2\2\2IJ\7"+
		"+\2\2J\f\3\2\2\2KL\7<\2\2L\16\3\2\2\2MN\7.\2\2N\20\3\2\2\2OP\7=\2\2P\22"+
		"\3\2\2\2QR\7x\2\2RS\7c\2\2ST\7t\2\2T\24\3\2\2\2UV\7~\2\2V\26\3\2\2\2W"+
		"X\7/\2\2XY\7@\2\2Y\30\3\2\2\2Z[\7\60\2\2[\\\7\60\2\2\\\32\3\2\2\2]^\7"+
		"?\2\2^\34\3\2\2\2_`\7-\2\2`a\7?\2\2a\36\3\2\2\2bc\7/\2\2cd\7?\2\2d \3"+
		"\2\2\2ef\7?\2\2fg\7?\2\2g\"\3\2\2\2hi\7#\2\2ij\7?\2\2j$\3\2\2\2kl\7@\2"+
		"\2l&\3\2\2\2mn\7>\2\2n(\3\2\2\2op\7@\2\2pq\7?\2\2q*\3\2\2\2rs\7>\2\2s"+
		"t\7?\2\2t,\3\2\2\2uv\7(\2\2vw\7(\2\2w.\3\2\2\2xy\7~\2\2yz\7~\2\2z\60\3"+
		"\2\2\2{|\7u\2\2|}\7m\2\2}~\7k\2\2~\177\7r\2\2\177\62\3\2\2\2\u0080\u0081"+
		"\7o\2\2\u0081\u0082\7c\2\2\u0082\u0083\7v\2\2\u0083\u0084\7e\2\2\u0084"+
		"\u0085\7j\2\2\u0085\64\3\2\2\2\u0086\u0087\7t\2\2\u0087\u0088\7g\2\2\u0088"+
		"\u0089\7v\2\2\u0089\u008a\7w\2\2\u008a\u008b\7t\2\2\u008b\u008c\7p\2\2"+
		"\u008c\66\3\2\2\2\u008d\u008e\7u\2\2\u008e\u008f\7g\2\2\u008f\u0090\7"+
		"t\2\2\u0090\u0091\7x\2\2\u0091\u0092\7g\2\2\u0092\u0093\7t\2\2\u00938"+
		"\3\2\2\2\u0094\u0095\7r\2\2\u0095\u0096\7t\2\2\u0096\u0097\7q\2\2\u0097"+
		"\u0098\7e\2\2\u0098\u0099\7g\2\2\u0099\u009a\7u\2\2\u009a\u009b\7u\2\2"+
		"\u009b:\3\2\2\2\u009c\u009e\t\2\2\2\u009d\u009c\3\2\2\2\u009e\u009f\3"+
		"\2\2\2\u009f\u009d\3\2\2\2\u009f\u00a0\3\2\2\2\u00a0<\3\2\2\2\u00a1\u00a3"+
		"\t\3\2\2\u00a2\u00a4\t\4\2\2\u00a3\u00a2\3\2\2\2\u00a4\u00a5\3\2\2\2\u00a5"+
		"\u00a3\3\2\2\2\u00a5\u00a6\3\2\2\2\u00a6>\3\2\2\2\u00a7\u00a9\t\5\2\2"+
		"\u00a8\u00a7\3\2\2\2\u00a9\u00aa\3\2\2\2\u00aa\u00a8\3\2\2\2\u00aa\u00ab"+
		"\3\2\2\2\u00ab\u00ac\3\2\2\2\u00ac\u00ad\b \2\2\u00ad@\3\2\2\2\6\2\u009f"+
		"\u00a5\u00aa\3\b\2\2";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}