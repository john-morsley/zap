var LogManager = Java.type('org.apache.logging.log4j.LogManager');
var logger = LogManager.getLogger('session.js');

var token = null;

function extractWebSession(sessionWrapper) {

  logger.info("[SESSION] \/ --------------------- EXTRACTING SESSION --------------------- \/");

  logger.info("[SESSION] Extracting web session...");

  // Extract token from the authentication response
  var msg = sessionWrapper.getHttpMessage();
  var responseBody = msg.getResponseBody().toString();
  
  logger.info("[SESSION] Response body length: " + responseBody.length);
  
  if (responseBody && responseBody.length > 0) {
    token = responseBody.trim();
    // Remove quotes if present
    token = token.replace(/^"(.*)"$/, '$1');
    sessionWrapper.getSession().setValue("token", token);
    logger.info("[SESSION] Token extracted and stored: " + token.substring(0, 20) + "...");
  } else {
    logger.warn("[SESSION] No token found in response");
  }

  logger.info("[SESSION] /\\ --------------------- EXTRACTING SESSION --------------------- /\\");
}

function processMessageToMatchSession(sessionWrapper) {

  logger.info("[SESSION] \/ --------------------- PROCESSING MESSAGE TO MATCH SESSION --------------------- \/");

  logger.info("[SESSION] Processing message to match session...");

  // Add the stored token to outgoing requests
  var msg = sessionWrapper.getHttpMessage();
  var session = sessionWrapper.getSession();
  var storedToken = session.getValue("token");
  
  if (storedToken) {
    logger.info("[SESSION] Adding Authorization header with token");
    msg.getRequestHeader().setHeader("Authorization", "Bearer " + storedToken);
  } else {
    logger.warn("[SESSION] No token available to add");
  }
  
  logger.info("[SESSION] /\\ --------------------- PROCESSING MESSAGE TO MATCH SESSION --------------------- /\\");

  return msg;
}

function getRequiredParamsNames() {
  return [];
}

function getOptionalParamsNames() {
  return [];
}

function clearWebSessionIdentifiers(sessionWrapper) {

  logger.info("[SESSION] \/ --------------------- CLEARING SESSION --------------------- \/");

  logger.info("[SESSION] Clearing web session identifiers...");

  var session = sessionWrapper.getSession();
  session.setValue("token", null);
  token = null;
  logger.info("[SESSION] Session cleared");

  logger.info("[SESSION] /\\ --------------------- CLEARING SESSION --------------------- /\\");
}
